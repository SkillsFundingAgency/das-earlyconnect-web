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
    [Route("name", Name = RouteNames.Name_Get, Order = 0)]
    public IActionResult Name(string? studentSurveyId)
    {
        var model = new NameViewModel 
        {
            StudentSurveyId = studentSurveyId
        };
        return View(model);
    }

    [HttpPost]
    [Route("name", Name = RouteNames.Name_Post, Order = 0)]
    public async Task<IActionResult> Name(NameViewModel model)
    {
        try
        {
            //var studentSurveyResponse = _mediator.Send(modelnew Create)


            //var response = await _mediator.Send(new CreateOtherStudent
            //{
            //    StudentTriageData = new OtherStudentTriageData()
            //    {
            //        Email = model.Email,
            //        LepsCode = model.LepsCode
            //    }
            //});

            //var authenticateViewModel = new AuthenticateViewModel
            //{
            //    AuthCode = response.AuthCode,
            //    ExpiryDate = response.ExpiryDate,
            //    StudentSurveyId = response.StudentSurveyId,
            //    LepsCode = model.LepsCode,
            //    Email = model.Email
            //};

            //TempData[TempDataKeys.TempDataAuthenticateModel] = JsonConvert.SerializeObject(authenticateViewModel);

            return RedirectToRoute(RouteNames.Authenticate_Get);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error posting email address");
            return BadRequest();
        }
    }

    private async Task SignInUser(String email, string StudentSurveyId)
    {
        // Create claims for the authenticated user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "user"),
            new Claim("StudentSurveyId", "StudentSurveyId")
            // Add any other claims as needed
        };

        var identity = new ClaimsIdentity(claims, "cookie");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        // Sign in the user
    }
}

