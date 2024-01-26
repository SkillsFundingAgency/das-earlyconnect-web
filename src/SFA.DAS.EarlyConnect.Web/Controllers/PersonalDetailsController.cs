using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Web.Mappers;

namespace SFA.DAS.EarlyConnect.Web.Controllers;


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

            var response = await _mediator.Send(new CreateStudentTriageDataCommand
            {
                StudentData = new StudentTriageData
                {
                    Id = studentSurveyResponse.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = studentSurveyResponse.DateOfBirth.GetValueOrDefault(),
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
                return RedirectToRoute(RouteNames.StartAgain_Get);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error posting Name");
            return BadRequest();
        }
    }
}

