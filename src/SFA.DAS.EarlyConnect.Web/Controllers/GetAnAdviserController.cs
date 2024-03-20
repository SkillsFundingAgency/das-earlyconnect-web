using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Domain.Configuration;
using Microsoft.Extensions.Options;

namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class GetAnAdviserController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetAnAdviserController> _logger;
    private readonly IUrlValidator _urlValidator;
    private readonly IDataProtectorService _dataProtectorService;
    private IAuthenticateService _authenticateService;
    private readonly IOptions<EarlyConnectWeb> _config;

    public GetAnAdviserController(IMediator mediator,
        ILogger<GetAnAdviserController> logger,
        IUrlValidator urlValidator,
        IDataProtectorService dataProtectorService,
        IOptions<EarlyConnectWeb> config,
        IAuthenticateService authenticateService
        )
    {
        _mediator = mediator;
        _logger = logger;
        _urlValidator = urlValidator;
        _dataProtectorService = dataProtectorService;
        _config = config;
        _authenticateService = authenticateService;
    }

    [HttpGet]
    [Route("{lepsCode}", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public IActionResult Index(string lepsCode)
    {
        return _urlValidator.IsValidLepsCode(lepsCode) ? View() : NotFound();
    }

    [HttpPost]
    [Route("{lepsCode}", Name = RouteNames.GetAnAdviser_Post, Order = 0)]
    public IActionResult GetAnAdviser_Post(string lepsCode)
    {
        return RedirectToRoute(RouteNames.Email_Get, new { lepsCode = lepsCode });

    }

    [HttpGet]
    [Route("ref", Name = RouteNames.UCASServiceStart_Get, Order = 0)]
    public async Task<IActionResult> UCASIndex()
    {
        try
        {
            var linkCode = Request.QueryString.Value.Substring(Request.QueryString.Value.IndexOf("?") + 1);
            var linkData = _dataProtectorService.DecodeData(linkCode).Split("|");

            if (!_urlValidator.IsValidLinkDate(linkData[1]))
            {
                return View("Expired", GetAdviserLinksModel());
            }

            var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = new Guid(linkData[0]) });
            if (result == null)
            {
                return View("LinkFault", GetAdviserLinksModel());
            }

            return View(new GetAdvisorViewModel { StudentSurveyId = new Guid(linkData[0]), Email = result.Email });
        }
        catch
        {
            return View("LinkFault", GetAdviserLinksModel());
        }
    }

    [HttpPost]
    [Route("ref", Name = RouteNames.GetAnAdviserUCAS_Post, Order = 0)]
    public async Task<IActionResult> GetAnAdviserUCAS_Post(GetAdvisorViewModel m)
    {
        await _authenticateService.SignInUser(m.Email, m.StudentSurveyId.ToString());

        return RedirectToRoute(RouteNames.PersonalDetails_Get, new { StudentSurveyId = m.StudentSurveyId });

    }

    [Route("cookies", Name = RouteNames.Cookies_Get)]
    public IActionResult Cookies()
    {
        return View();
    }

    [Route("cookies-details", Name = RouteNames.CookiesDetails_Get)]
    public IActionResult CookiesDetails()
    {
        return View();
    }

    private AdviserLinksViewModel GetAdviserLinksModel()
    {
        return new AdviserLinksViewModel
        {
            GreaterLondonLEPSCode = _config.Value.LepCodes.GreaterLondon,
            LancashireLEPSCode = _config.Value.LepCodes.Lancashire,
            NorthEastLEPSCode = _config.Value.LepCodes.NorthEast
        };
    }
}

