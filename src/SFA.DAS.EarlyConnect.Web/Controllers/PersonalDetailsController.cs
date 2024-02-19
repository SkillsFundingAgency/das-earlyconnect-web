using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
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
        ClearModelState();
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

        string routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.ApprenticeshipLevel_Get;

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
    public async Task<IActionResult> Name(TriageRouteModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        return View(new NameViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
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
                StudentData = model.MapFromNameRequest(studentSurveyResponse),
                SurveyGuid = model.StudentSurveyId
            });

            var routeName = model.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.DateOfBirth_Get;

            return RedirectToRoute(routeName, new { model.StudentSurveyId });
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

    [HttpGet]
    [Route("dateofbirth", Name = RouteNames.DateOfBirth_Get, Order = 0)]
    public async Task<IActionResult> DateOfBirth(TriageRouteModel m)
    {
        var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = m.StudentSurveyId });

        return View(new DateOfBirthEditViewModel
        {
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck,
            Day = $"{ (result.DateOfBirth.HasValue ? result.DateOfBirth.Value.Day : string.Empty)}",
            Month = $"{(result.DateOfBirth.HasValue ? result.DateOfBirth.Value.Month : string.Empty)}",
            Year = $"{(result.DateOfBirth.HasValue ? result.DateOfBirth.Value.Year : string.Empty)}",
        });
    }

    [HttpPost]
    [Route("DateOfBirth", Name = RouteNames.DateOfBirth_Post, Order = 0)]
    public async Task<IActionResult> DateOfBirth(DateOfBirthEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromDateOfBirthRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        var routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.Postcode_Get;

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

    [HttpGet]
    [Route("industry", Name = RouteNames.Industry_Get, Order = 0)]
    public async Task<IActionResult> Industry(Guid studentSurveyId, bool? isSummaryReview)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = studentSurveyId
        });

        IndustryViewModel viewModel = new IndustryViewModel
        {
            StudentSurveyId = studentSurveyId,
            Areas = studentSurveyResponse.Industry.Split("|").ToList(),
            IsCheck = isSummaryReview.GetValueOrDefault()
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("industry", Name = RouteNames.Industry_Post, Order = 0)]
    public async Task<IActionResult> Industry(IndustryViewModel model)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = model.StudentSurveyId
        });

        var response = await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = model.MapFromIndustryRequest(studentSurveyResponse),
            SurveyGuid = model.StudentSurveyId
        });


        var routeName = model.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.SchoolName_Get;

        return RedirectToRoute(routeName, new { model.StudentSurveyId });

    }
    private void ClearModelState()
    {
        if (ModelState.ContainsKey("Question.Answers"))
        {
            ModelState.Clear();
        }
    }
}

