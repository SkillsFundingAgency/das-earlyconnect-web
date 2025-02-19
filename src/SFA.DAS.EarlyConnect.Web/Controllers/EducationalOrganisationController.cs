using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Encodings.Web;

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
    [Route("/educationalOrganisations/search")]
    public async Task<IActionResult> GetEducationalOrganisationsBySearch([FromQuery] string searchTerm, [FromQuery] string lepCode)
    {
        var educationalOrganisationsResponse = await _mediator.Send(new GetEducationalOrganisationsQuery
        {
            SearchTerm = searchTerm,
            LepCode = lepCode,
        });

        return Ok(educationalOrganisationsResponse.EducationalOrganisations);
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

        SanitizeModelState("SchoolSearchTerm");

        return View(new SearchSchoolEditViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            SchoolSearchTerm = result.SchoolName,
            ExistingSchool = result.SchoolName,
            LepCode = result.LepCode,
            SelectedUrn = result.URN,
            IsOther = result.DataSource == Datasource.Others
        });

    }

    [HttpPost]
    [Route("searchschool", Name = RouteNames.SearchSchool_Post)]
    public async Task<IActionResult> SearchSchool(SearchSchoolEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });
        var educationalOrganisationsResponse = await _mediator.Send(new GetEducationalOrganisationsQuery
        {
            SearchTerm = m.SchoolSearchTerm,
            LepCode = studentSurveyResponse.LepCode,
            Page = 1,
            PageSize = 1,
        });

        string routeName;

        if (m.IsJsEnabled)
        {
            await _mediator.Send(new CreateStudentTriageDataCommand
            {
                StudentData = m.MapFromSearchSchoolNameRequest(studentSurveyResponse),
                SurveyGuid = m.StudentSurveyId
            });
            routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.ApprenticeshipLevel_Get;
        }
        else if (m.SchoolSearchTerm == studentSurveyResponse.SchoolName)
        {
            routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.ApprenticeshipLevel_Get;
        }
        else
        {
            routeName = educationalOrganisationsResponse?.EducationalOrganisations != null && educationalOrganisationsResponse.TotalCount > 0
                ? RouteNames.SelectSchool_Get
                : RouteNames.NoResultsFound_Get;
        }

        object routeValues;

        if (routeName == RouteNames.SelectSchool_Get || routeName == RouteNames.NoResultsFound_Get)
        {
            routeValues = new
            {
                studentSurveyId = m.StudentSurveyId,
                schoolSearchTerm = m.SchoolSearchTerm,
                isCheck = m.IsCheck,
                page = 1,
                pageSize = 10
            };
        }
        else
        {
            routeValues = new { studentSurveyId = m.StudentSurveyId };
        }

        return RedirectToRoute(routeName, routeValues);
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
            Page = m.Page,
            PageSize = m.PageSize,
        });

        var educationalOrganisations = (SelectSchoolEditViewModel)educationalOrganisationsResponse;

        educationalOrganisations.SchoolSearchTerm = m.SchoolSearchTerm;
        educationalOrganisations.StudentSurveyId = m.StudentSurveyId;
        educationalOrganisations.IsCheck = m.IsCheck;
        educationalOrganisations.Page = m.Page;
        educationalOrganisations.PageSize = m.PageSize;
        educationalOrganisations.IsOther = result.DataSource == Datasource.Others;

        var pagingUrl = GeneratePagingUrl(m);

        educationalOrganisations.PaginationViewModel = SetupPagination(educationalOrganisations, pagingUrl);

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

        return RedirectToRoute(routeName, new { studentSurveyId = m.StudentSurveyId });
    }

    [HttpGet]
    [Route("noresultsfound", Name = RouteNames.NoResultsFound_Get)]
    public IActionResult NoResultsFound(NoResultsFoundViewModel m)
    {
        return View(new NoResultsFoundViewModel()
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            SchoolSearchTerm = m.SchoolSearchTerm,
        });
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

        return RedirectToRoute(routeName, new { studentSurveyId = m.StudentSurveyId });
    }

    private void SanitizeModelState(string key)
    {
        if (ModelState.ContainsKey(key))
        {
            var unsafeValue = ModelState[key]?.AttemptedValue;
            if (!string.IsNullOrEmpty(unsafeValue))
            {
                var sanitizedValue = HtmlEncoder.Default.Encode(unsafeValue);
                ModelState.SetModelValue(key, new ValueProviderResult(sanitizedValue));
            }
        }
    }

    private static PaginationViewModel SetupPagination(SelectSchoolEditViewModel request, string filterUrl)
    {
        var totalPages = (request.TotalCount > request.PageSize) ? (int)Math.Ceiling((double)request.TotalCount / request.PageSize) : 1;

        var pagination = new PaginationViewModel(request.Page, request.PageSize, totalPages, filterUrl);

        return pagination;
    }

    private string GeneratePagingUrl(SelectSchoolViewModel model)
    {
        var queryParams = new Dictionary<string, string?>
        {
            { "studentSurveyId", model.StudentSurveyId.ToString() },
            { "schoolSearchTerm", model.SchoolSearchTerm },
            { "isCheck", model.IsCheck.ToString() },
        };

        return QueryHelpers.AddQueryString("/selectschool", queryParams);
    }

    private void ClearModelState()
    {
        if (ModelState.ContainsKey("Question.Answers"))
        {
            ModelState.Clear();
        }
    }
}

