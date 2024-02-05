using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Web.Mappers;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.Controllers;

[Authorize]
public class PersonalDetailsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<PersonalDetailsController> _logger;

    public PersonalDetailsController(IMediator mediator,
        ILogger<PersonalDetailsController> logger
        )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("schoolname", Name = RouteNames.SchoolName_Get, Order = 0)]
    public async Task<IActionResult> SchoolName(SchoolNameViewModel m)
    {
        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });

        return View(new SchoolNameEditViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            SchoolName = result.SchoolName,
            IsOther = result.DataSource == Datasource.Others
        });
    }

    [HttpPost]
    [Route("schoolname", Name = RouteNames.SchoolName_Post, Order = 0)]
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

        string routeName = m.IsOther
            ? (m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.ApprenticeshipLevel_Get)
            : (m.IsCheck ? RouteNames.CheckYourAnswersDummy_Get : RouteNames.ApprenticeshipLevel_Get);

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("postcode", Name = RouteNames.Postcode_Get, Order = 0)]
    public async Task<IActionResult> Postcode(PostcodeViewModel m)
    {
        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });

        return View(new PostcodeEditViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            Postcode = result.Postcode
        });
    }

    [HttpPost]
    [Route("postcode", Name = RouteNames.Postcode_Post, Order = 0)]
    public async Task<IActionResult> Postcode(PostcodeEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromPostcodeRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        var routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.Telephone_Get;

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("name", Name = RouteNames.Name_Get, Order = 0)]
    public async Task<IActionResult> Name(Guid studentSurveyId, bool? isSummaryReview)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = studentSurveyId
        });

        return View(new NameViewModel
        {
            StudentSurveyId = studentSurveyId,
            IsCheck = isSummaryReview.GetValueOrDefault(),
            FirstName = studentSurveyResponse.FirstName,
            LastName = studentSurveyResponse.LastName
        });
    }

    [HttpPost]
    [Route("name", Name = RouteNames.Name_Post, Order = 0)]
    public async Task<IActionResult> Name(NameViewModel model)
    {
        try
        {
            var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
            {
                SurveyGuid = model.StudentSurveyId
            });

            studentSurveyResponse.StudentSurvey.ResponseAnswers = new List<ResponseAnswersDto>();

            var response = await _mediator.Send(new CreateStudentTriageDataCommand
            {
                StudentData = new StudentTriageData
                {
                    Id = studentSurveyResponse.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SchoolName = studentSurveyResponse.SchoolName,
                    DateOfBirth = studentSurveyResponse.DateOfBirth,
                    Email = studentSurveyResponse.Email,
                    Postcode = studentSurveyResponse.Postcode,
                    Telephone = studentSurveyResponse.Telephone,
                    DataSource = studentSurveyResponse.DataSource,
                    Industry = studentSurveyResponse.Industry,
                    StudentSurvey = studentSurveyResponse.StudentSurvey
                },
                SurveyGuid = model.StudentSurveyId
            });

            if (model.IsCheck)
            {
                return RedirectToRoute(RouteNames.CheckYourAnswers_Get, new { model.StudentSurveyId });
            }
            else
            {
                return RedirectToRoute(RouteNames.Postcode_Get, new { model.StudentSurveyId });
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error posting Name");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("telephone", Name = RouteNames.Telephone_Get)]
    public async Task<IActionResult> Telephone(TriageRouteModel m)
    {
        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });

        return View(new TelephoneEditViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            Telephone = result.Telephone
        });
    }

    [HttpPost]
    [Route("telephone", Name = RouteNames.Telephone_Post)]
    public async Task<IActionResult> Telephone(TelephoneEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        if (m.Telephone != null)
        {
            var response = await _mediator.Send(new CreateStudentTriageDataCommand
            {
                StudentData = m.MapFromTelephoneRequest(studentSurveyResponse),
                SurveyGuid = m.StudentSurveyId
            });
        }

        var routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.Industry_Get;

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }
}

