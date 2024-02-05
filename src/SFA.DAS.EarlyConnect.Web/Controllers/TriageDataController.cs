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
using SFA.DAS.EarlyConnect.Web.RouteModel;
using SFA.DAS.EarlyConnect.Web.Mappers;

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
            SurveyGuid = model.StudentSurveyId
        });


        var routeName = model.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.School_Get;

        return RedirectToRoute(routeName, new { model.StudentSurveyId });

    }

    [HttpGet]
    [Route("move", Name = RouteNames.Move_Get, Order = 0)]
    public async Task<IActionResult> Move(TriageRouteModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        var relocateQuestion = studentSurveyResponse.SurveyQuestions.Where(q => q.SortOrder == 4).First();
        var studentAnswers = studentSurveyResponse.StudentSurvey.ResponseAnswers.Where(a => a.QuestionId == relocateQuestion.Id).Select(a => a.AnswerId.Value).ToList();

        MoveEditViewModel editModel = new MoveEditViewModel()
        {
            QuestionId = relocateQuestion.Id,
            QuestionText = relocateQuestion.QuestionText,
            Answers = relocateQuestion.Answers.ToList(),
            SelectedAnswerId = studentAnswers,
            StudentSurveyId = m.StudentSurveyId,
            IsCheck = m.IsCheck
        };

        return View(editModel);
    }

    [HttpPost]
    [Route("move", Name = RouteNames.Move_Post, Order = 0)]
    public async Task<IActionResult> Move(MoveEditViewModel m)
    {
        var studentSurveyResponse = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery
        {
            SurveyGuid = m.StudentSurveyId
        });

        await _mediator.Send(new CreateStudentTriageDataCommand
        {
            StudentData = m.MapFromMoveRequest(studentSurveyResponse),
            SurveyGuid = m.StudentSurveyId
        });

        var routeName = m.IsCheck ? RouteNames.CheckYourAnswers_Get : RouteNames.Support_Get;

        return RedirectToRoute(routeName, new { m.StudentSurveyId });
    }

}

