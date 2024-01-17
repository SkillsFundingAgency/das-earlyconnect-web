using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;


namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class GetAnAdviserController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetAnAdviserController> _logger;

    public GetAnAdviserController(IMediator mediator, ILogger<GetAnAdviserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public async Task<IActionResult> Index()
    {
        string surveyGuid = "FF8E9040-3167-4D49-E408-08DC12C62C9B";

        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = surveyGuid });

        return View((GetStudentTriageDataBySurveyIdResponse)result);
    }
}

