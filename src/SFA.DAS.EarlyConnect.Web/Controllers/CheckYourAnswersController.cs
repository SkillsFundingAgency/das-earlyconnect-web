using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace das_earlyconnect_web.Controllers
{
    [Authorize]
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

            if (studentSurveyResponse.StudentSurvey.DateCompleted.HasValue)
            {
                return RedirectToRoute(RouteNames.FormCompleted_Get);
            }

            var checkYourAnswersViewModel = (CheckYourAnswersViewModel)studentSurveyResponse;
            checkYourAnswersViewModel.StudentSurveyId = studentSurveyId;
            checkYourAnswersViewModel.IsCheck = true;
            checkYourAnswersViewModel.IsOther = studentSurveyResponse.DataSource == Datasource.Others;

            return View(checkYourAnswersViewModel);
        }
        [HttpPost]
        [Route("check", Name = RouteNames.CheckYourAnswers_Post, Order = 0)]
        public async Task<IActionResult> Check(CheckYourAnswersViewModel m)
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

            return RedirectToRoute(RouteNames.Confirmation_Get, new { m.StudentSurveyId });
        }
    }
}
