using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Mappers;
using SFA.DAS.EarlyConnect.Web.Mappers.SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.EarlyConnect.Web.Controllers;

[Authorize]
public class SurveyController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<SurveyController> _logger;

    public SurveyController(IMediator mediator,
        ILogger<SurveyController> logger

        )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("apprenticeshiplevel", Name = RouteNames.ApprenticeshipLevel_Get, Order = 0)]
    public async Task<IActionResult> ApprenticeshipLevel(ApprenticeshiplevelViewModel m)
    {
        ClearModelState();
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        if (studentSurveyResponse.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        var apprenticeshiplevelEditViewModel = (ApprenticeshipLevelEditViewModel)studentSurveyResponse;
        apprenticeshiplevelEditViewModel.StudentSurveyId = m.StudentSurveyId;
        apprenticeshiplevelEditViewModel.IsCheck = m.IsCheck;
        apprenticeshiplevelEditViewModel.IsOther = studentSurveyResponse.DataSource == Datasource.Others;

        return View(apprenticeshiplevelEditViewModel);

    }

    [HttpPost]
    [Route("apprenticeshiplevel", Name = RouteNames.ApprenticeshipLevel_Post, Order = 0)]
    public async Task<IActionResult> ApprenticeshipLevel(ApprenticeshipLevelEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        if (studentSurveyResponse.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        m = MapViewModel(m, studentSurveyResponse, SurveyPage.Page.Apprenticeshiplevel);

        if (!ValidateAnswers<ApprenticeshipLevelEditViewModel>(m, vm => vm.Question.Answers, m.Question.ValidationMessage))
        {
            return View(m);
        }

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromApprenticeshiplevelRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        string routeName = m.IsOther
            ? (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.AppliedFor_Get)
            : (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.AppliedFor_Get);

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("appliedfor", Name = RouteNames.AppliedFor_Get, Order = 0)]
    public async Task<IActionResult> AppliedFor(AppliedForViewModel m)
    {
        ClearModelState();

        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        if (studentSurveyResponse.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        var appliedForEditViewModel = (AppliedForEditViewModel)studentSurveyResponse;
        appliedForEditViewModel.StudentSurveyId = m.StudentSurveyId;
        appliedForEditViewModel.IsCheck = m.IsCheck;
        appliedForEditViewModel.IsOther = studentSurveyResponse.DataSource == Datasource.Others;

        return View(appliedForEditViewModel);

    }

    [HttpPost]
    [Route("appliedfor", Name = RouteNames.AppliedFor_Post, Order = 0)]
    public async Task<IActionResult> AppliedFor(AppliedForEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        m = MapViewModel(m, studentSurveyResponse, SurveyPage.Page.AppliedFor);

        if (!ValidateAnswers<AppliedForEditViewModel>(m, vm => vm.Question.Answers, m.Question.ValidationMessage))
        {
            return View(m);
        }

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromAppliedForRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        string routeName = m.IsOther
            ? (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.Relocate_Get)
            : (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.Relocate_Get);

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("support", Name = RouteNames.Support_Get, Order = 0)]
    public async Task<IActionResult> Support(SupportViewModel m)
    {
        ClearModelState();

        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        if (studentSurveyResponse.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        var supportEditViewModel = (SupportEditViewModel)studentSurveyResponse;
        supportEditViewModel.StudentSurveyId = m.StudentSurveyId;
        supportEditViewModel.IsCheck = m.IsCheck;
        supportEditViewModel.IsOther = studentSurveyResponse.DataSource == Datasource.Others;

        return View(supportEditViewModel);
    }

    [HttpPost]
    [Route("support", Name = RouteNames.Support_Post, Order = 0)]
    public async Task<IActionResult> Support(SupportEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        m = MapViewModel(m, studentSurveyResponse, SurveyPage.Page.Support);

        if (!ValidateAnswers<SupportEditViewModel>(m, vm => vm.Question.Answers, m.Question.ValidationMessage, 3))
        {
            return View(m);
        }

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromSupportRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        string routeName = m.IsOther
            ? (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.CheckYourAnswers_Get)
            : (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.CheckYourAnswers_Get);

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("relocate", Name = RouteNames.Relocate_Get, Order = 0)]
    public async Task<IActionResult> Relocate(TriageRouteModel m)
    {
        ClearModelState();

        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        if (studentSurveyResponse.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        var relocateEditViewModel = (RelocateEditViewModel)studentSurveyResponse;
        relocateEditViewModel.StudentSurveyId = m.StudentSurveyId;
        relocateEditViewModel.IsCheck = m.IsCheck;
        relocateEditViewModel.IsOther = studentSurveyResponse.DataSource == Datasource.Others;

        return View(relocateEditViewModel);
    }

    [HttpPost]
    [Route("relocate", Name = RouteNames.Relocate_Post, Order = 0)]
    public async Task<IActionResult> Relocate(RelocateEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        foreach (var answer in m.Question.Answers.Where(a => a.Id == m.SelectedAnswerId))
        {
            answer.IsSelected = true;
        }

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromRelocateRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        string routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.Support_Get;

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("confirmation", Name = RouteNames.Confirmation_Get, Order = 0)]
    public async Task<IActionResult> Confirmation()
    {
        return View();
    }

    private bool ValidateAnswers<T>(T m, Func<T, List<Answers>> answersSelector, string validationMessage, int? minSelectedCount = null)
    {
        var answers = answersSelector(m);

        if (minSelectedCount.HasValue && (answers == null || answers.Count(a => a.IsSelected) > minSelectedCount))
        {
            ModelState.AddModelError("Question.Answers", validationMessage);
            return false;
        }

        if (answers == null || !answers.Any(a => a.IsSelected))
        {
            ModelState.AddModelError("Question.Answers", validationMessage);
            return false;
        }

        var hasSelectedDefault = answers.Any(a => a.IsSelected && a.ToggleAnswer == AnswerGroup.Group.Default);
        var hasSelectedToggled = answers.Any(a => a.IsSelected && a.ToggleAnswer == AnswerGroup.Group.Toggled);

        if (hasSelectedDefault && hasSelectedToggled)
        {
            ModelState.AddModelError("Question.Answers", validationMessage);
        }

        return ModelState.IsValid;
    }

    private void ClearModelState()
    {
        if (ModelState.ContainsKey("Question.Answers"))
        {
            ModelState.Clear();
        }
    }

    private T MapViewModel<T>(T m, GetStudentTriageDataBySurveyIdResult request, SurveyPage.Page surveyPage)
    where T : BaseSurveyViewModel
    {
        var surveyQuestion = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)surveyPage);

        if (surveyQuestion != null)
        {
            m.Question.ValidationMessage = surveyQuestion.ValidationMessage;
            m.Question.QuestionText = surveyQuestion.QuestionText;
            m.Question.ShortDescription = surveyQuestion.ShortDescription;
            m.Question.DefaultToggleAnswerId = surveyQuestion.DefaultToggleAnswerId;

            var existingAnswers = m.Question.Answers;

            int serialCounter = 0;
            foreach (var existingAnswer in existingAnswers)
            {
                var matchingAnswer = surveyQuestion.Answers.FirstOrDefault(answer => answer.Id == existingAnswer.Id);

                existingAnswer.GroupLabel = matchingAnswer?.GroupLabel;
                existingAnswer.AnswerText = matchingAnswer?.AnswerText;
                existingAnswer.ShortDescription = matchingAnswer?.ShortDescription;
                existingAnswer.StudentAnswerId = matchingAnswer?.Id ?? 0;

                existingAnswer.Serial = serialCounter;
                serialCounter++;

                existingAnswer.ToggleAnswer = existingAnswer.Id == surveyQuestion.DefaultToggleAnswerId
                    ? AnswerGroup.Group.Toggled
                    : AnswerGroup.Group.Default;
            }

            m.Question.Answers = existingAnswers;
        }

        return m;
    }
}

