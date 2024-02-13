using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Web.Extensions;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class GetAnAdviserController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetAnAdviserController> _logger;
    private readonly IUrlValidator _urlValidator;
    private readonly IDataProtectorService _dataProtectorService;

    public GetAnAdviserController(IMediator mediator,
        ILogger<GetAnAdviserController> logger,
        IUrlValidator urlValidator,
        IDataProtectorService dataProtectorService
        )
    {
        _mediator = mediator;
        _logger = logger;
        _urlValidator = urlValidator;
        _dataProtectorService = dataProtectorService;
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
    [Route("ucas/{code}", Name = RouteNames.UCASServiceStart_Get, Order = 0)]
    public async Task<IActionResult> UCASIndex(string code)
    {
        var linkData = _dataProtectorService.DecodeData(code).Split("|");

        if (!_urlValidator.IsValidLinkDate(linkData[1]))
        { 
            return View ("Expired");
        }

        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = new Guid(linkData[0]) });
        if (result == null)
        {
            return View("LinkFault");
        }

        return View(new TriageRouteModel { StudentSurveyId = new Guid(linkData[0]) });
    }

    [HttpPost]
    [Route("ucas", Name = RouteNames.GetAnAdviserUCAS_Post, Order = 0)]
    public IActionResult GetAnAdviserUCAS_Post(TriageRouteModel m)
    {
        return RedirectToRoute(RouteNames.UCASDetails_Get, new { studentSurveyId = m.StudentSurveyId });
    }
}

