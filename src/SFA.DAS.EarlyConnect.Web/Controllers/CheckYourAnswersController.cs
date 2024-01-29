using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace das_earlyconnect_web.Controllers
{
    public class CheckYourAnswersController:Controller
    {

        private readonly IMediator _mediator;
        private readonly ILogger<CheckYourAnswersController> _logger;

        public CheckYourAnswersController(IMediator mediator,
            ILogger<CheckYourAnswersController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("check", Name = RouteNames.CheckYourAnswers_Get, Order = 0)]
        public IActionResult Check(string? studentSurveyId)
        {
            CheckYourAnswersViewModel model = new CheckYourAnswersViewModel
            { 
                StudentSurveyId = studentSurveyId
            };
            return View(model);
        }

    }
}
