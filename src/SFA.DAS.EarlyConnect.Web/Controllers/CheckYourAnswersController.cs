using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace das_earlyconnect_web.Controllers
{
    public class CheckYourAnswersController : Controller
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
        public async Task<IActionResult> Check(Guid studentSurveyId)
        {
            var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
            {
                SurveyGuid = studentSurveyId
            });
            var checkYourAnswersViewModel = (CheckYourAnswersViewModel)studentSurveyResponse;
            checkYourAnswersViewModel.StudentSurveyId = studentSurveyId;
            //checkYourAnswersViewModel.IsCheck = m.IsCheck;

            return View(checkYourAnswersViewModel);
        }
        [HttpPost]
        [Route("check", Name = RouteNames.CheckYourAnswers_Post, Order = 0)]
        public async Task<IActionResult> ApprenticeshipLevel(CheckYourAnswersViewModel m)
        {
            var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
            {
                SurveyGuid = m.StudentSurveyId
            });

            var response = await _mediator.Send(new CreateStudentTriageDataCommand
            {
                StudentData = m.MapFromCheckYourAnswersRequest(studentSurveyResponse),
                SurveyGuid = m.StudentSurveyId
            });

            string routeName = m.IsOther
                ? (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.AppliedFor_Get)
                : (m.IsCheck ? RouteNames.CheckYourAnswersDummy_Get : RouteNames.AppliedFor_Post);

            return RedirectToRoute(routeName, new { m.StudentSurveyId });
        }
    }
}
