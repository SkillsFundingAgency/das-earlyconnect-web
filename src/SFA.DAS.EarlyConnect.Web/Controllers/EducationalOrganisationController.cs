using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Mappers;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations;

namespace SFA.DAS.EarlyConnect.Web.Controllers;

[Authorize]
public class EducationalOrganisationController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<EducationalOrganisationController> _logger;

    public EducationalOrganisationController(IMediator mediator,
        ILogger<EducationalOrganisationController> logger
        )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("searchschool", Name = RouteNames.SearchSchool_Get)]
    public async Task<IActionResult> SearchSchool(SearchSchoolViewModel m)
    {
        ClearModelState();
        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });

        if (result.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        return View(new SearchSchoolEditViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            SchoolSearchTerm = result.SchoolName,
            IsOther = result.DataSource == Datasource.Others
        });

    }

    [HttpPost]
    [Route("searchschool", Name = RouteNames.SearchSchool_Post)]
    public async Task<IActionResult> SearchSchool(SearchSchoolEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        if (m.SchoolSearchTerm == studentSurveyResponse.SchoolName)
        {
            var routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.ApprenticeshipLevel_Get;
            return RedirectToRoute(routeName, new { m.StudentSurveyId });
        }

        var educationalOrganisationsResponse = await _mediator.Send(new GetEducationalOrganisationsQuery
        {
            SearchTerm = m.SchoolSearchTerm,
            LepCode = studentSurveyResponse.LepCode,
            Page = 1,
            PageSize = 1,
        });

        var finalRouteName = (educationalOrganisationsResponse?.EducationalOrganisations != null
                              && educationalOrganisationsResponse.TotalCount > 0)
            ? RouteNames.SelectSchool_Get
            : RouteNames.NoResultsFound_Get;

        return RedirectToRoute(finalRouteName, new { m.StudentSurveyId, m.SchoolSearchTerm, m.IsCheck });
    }

    [HttpGet]
    [Route("selectschool", Name = RouteNames.SelectSchool_Get)]
    public async Task<IActionResult> SelectSchool(SelectSchoolViewModel m)
    {
        ClearModelState();
        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });

        if (result.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        var educationalOrganisationsResponse = await _mediator.Send(new GetEducationalOrganisationsQuery
        {
            SearchTerm = m.SchoolSearchTerm,
            LepCode = result.LepCode,
            Page = 1,
            PageSize = 10,
        });

        var educationalOrganisations = (SelectSchoolEditViewModel)educationalOrganisationsResponse;

        educationalOrganisations.SchoolSearchTerm = m.SchoolSearchTerm;
        educationalOrganisations.StudentSurveyId = m.StudentSurveyId;
        educationalOrganisations.IsCheck = m.IsCheck;
        educationalOrganisations.IsOther = result.DataSource == Datasource.Others;

        return View(educationalOrganisations);
    }

    [HttpPost]
    [Route("selectschool", Name = RouteNames.SelectSchool_Post)]
    public async Task<IActionResult> SelectSchool(SelectSchoolEditViewModel m)
    {
        var parts = m.SelectedSchool.Split(',');

        if (parts.Length == 2)
        {
            m.SelectedSchool = parts[0];
            m.SelectedURN = parts[1];
        }

        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromSelectSchoolNameRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        string routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.ApprenticeshipLevel_Get;

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("noresultsfound", Name = RouteNames.NoResultsFound_Get)]
    public IActionResult NoResultsFound()
    {
        return View();
    }

    [HttpGet]
    [Route("schoolname", Name = RouteNames.SchoolName_Get)]
    public async Task<IActionResult> SchoolName(SchoolNameViewModel m)
    {
        ClearModelState();
        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });

        if (result.StudentSurvey.DateCompleted.HasValue)
        {
            return RedirectToRoute(RouteNames.FormCompleted_Get);
        }

        return View(new SchoolNameEditViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            SchoolName = result.SchoolName,
            IsOther = result.DataSource == Datasource.Others
        });
    }

    [HttpPost]
    [Route("schoolname", Name = RouteNames.SchoolName_Post)]
    public async Task<IActionResult> SchoolName(SchoolNameEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromSchoolNameRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        string routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.ApprenticeshipLevel_Get;

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }
    private void ClearModelState()
    {
        if (ModelState.ContainsKey("Question.Answers"))
        {
            ModelState.Clear();
        }
    }
}

