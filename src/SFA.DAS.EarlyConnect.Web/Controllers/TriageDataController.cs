using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using Newtonsoft.Json;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Web.Configuration;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using System.Reflection;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;

namespace SFA.DAS.EarlyConnect.Web.Controllers;


public class TriageDataController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<TriageDataController> _logger;

    public TriageDataController(IMediator mediator,
        ILogger<TriageDataController> logger
        
        )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("industry", Name = RouteNames.Industry_Get, Order = 0)]
    public async Task<IActionResult> Industry(string ? studentSurveyId, bool? isSummaryReview)
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
    public async Task<IActionResult> Industry(IndustryViewModel model, List<string> sector)
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
                FirstName = studentSurveyResponse.FirstName,
                LastName = studentSurveyResponse.LastName,
                DateOfBirth = studentSurveyResponse.DateOfBirth.GetValueOrDefault(),
                Email = studentSurveyResponse.Email,
                Postcode = studentSurveyResponse.Postcode,
                Telephone = studentSurveyResponse.Telephone,
                DataSource = studentSurveyResponse.DataSource,
                Industry = string.Join("|", sector),
                StudentSurvey = studentSurveyResponse.StudentSurvey
            },
            SurveyGuid = new Guid(model.StudentSurveyId)
        });

        if (model.IsCheck)
        {
            return RedirectToRoute(RouteNames.CheckYourAnswers_Get, new { studentSurveyId = model.StudentSurveyId });
        }
        else
        {
            return RedirectToRoute(RouteNames.Dummy, new { studentSurveyId = model.StudentSurveyId });
        }
    }

}

