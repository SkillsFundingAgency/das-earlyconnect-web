using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
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
        IAuthenticateService authenticateService
        )
    {
        _mediator = mediator;
        _logger = logger;
        _urlValidator = urlValidator;
        _authenticateService = authenticateService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("Index");
    }
}

