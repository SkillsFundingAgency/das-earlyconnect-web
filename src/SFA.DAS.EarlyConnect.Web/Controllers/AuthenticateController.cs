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


public class AuthenticateController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticateController> _logger;
    private readonly IUrlValidator _urlValidator;
    private readonly IDataProtectorService _dataProtectorService;
    private const string TempDataAuthenticateModel = "AuthenticateModel";

    public AuthenticateController(IMediator mediator,
        ILogger<AuthenticateController> logger,
        IUrlValidator urlValidator,
        IDataProtectorService dataProtectorService
        )
    {
        _mediator = mediator;
        _logger = logger;
        _urlValidator = urlValidator;
        _dataProtectorService = dataProtectorService;
    }

    [HttpGet]
    [Route("authenticate", Name = RouteNames.Authenticate_Get, Order = 0)]
    public IActionResult Authenticate()
    {
        if (TempData[TempDataAuthenticateModel] is string model)
        {
            var viewModel = JsonConvert.DeserializeObject<AuthenticateViewModel>(model);
            if (_urlValidator.IsValidLepsCode(viewModel.LepsCode))
            {
                TempData[TempDataAuthenticateModel] = JsonConvert.SerializeObject(viewModel);

                var authCodeViewModel = new AuthCodeViewModel()
                {
                    AuthCode = viewModel.AuthCode
                };

                return View(authCodeViewModel);
            }
        }

        return NotFound();
    }

    [HttpPost]
    [Route("authenticate", Name = RouteNames.Authenticate_Post, Order = 0)]
    public async Task<IActionResult> Authenticate(AuthCodeViewModel request)
    {
        if (TempData[TempDataAuthenticateModel] is string model)
        {
            var viewModel = JsonConvert.DeserializeObject<AuthenticateViewModel>(model);
            var decryptedAuthCode = _dataProtectorService.DecodeData(viewModel.AuthCode);

            if (request.AuthCode != decryptedAuthCode)
            {
                ModelState.AddModelError(nameof(request.AuthCode), "Enter the correct confirmation code");

                return View(request);
            }

            if (viewModel.ExpiryDate > DateTime.Now)
            {
                return RedirectToRoute(RouteNames.StartAgain_Get, new { viewModel.LepsCode });
            }

            await SignInUser(viewModel.Email, viewModel.StudentSurveyId);

            return RedirectToRoute(RouteNames.Name_Get, new { viewModel.StudentSurveyId });
        }

        return NotFound();
    }

    [HttpGet]
    [Route("start-again", Name = RouteNames.StartAgain_Get, Order = 0)]
    public IActionResult StartAgain(string lepsCode)
    {
        return View(lepsCode);
    }

    [HttpGet]
    [Route("send-code", Name = RouteNames.SendCode_Post, Order = 0)]
    public async Task<IActionResult> SendCode(string authcode)
    {
        if (TempData[TempDataAuthenticateModel] is string model)
        {
            var viewModel = JsonConvert.DeserializeObject<AuthenticateViewModel>(model);
            var response = await _mediator.Send(new CreateOtherStudentTriageDataCommand
            {
                StudentTriageData = new OtherStudentTriageData
                {
                    Email = viewModel.Email,
                    LepsCode = viewModel.LepsCode
                }
            });
            var authenticateViewModel = new AuthenticateViewModel
            {
                AuthCode = response.AuthCode,
                Email = viewModel.Email,
                ExpiryDate = response.ExpiryDate,
                LepsCode = viewModel.LepsCode,
                StudentSurveyId = viewModel.StudentSurveyId
            };
            TempData[TempDataAuthenticateModel] = JsonConvert.SerializeObject(authenticateViewModel);
            return RedirectToRoute(RouteNames.Authenticate_Get, new { viewModel.LepsCode });
        }

        return NotFound();
    }

    [HttpGet]
    [Route("email", Name = RouteNames.Email_Get, Order = 0)]
    public IActionResult EmailAddress(string? lepsCode)
    {
        var model = new EmailAddressViewModel
        {
            LepsCode = lepsCode
        };
        return View(model);
    }

    [HttpPost]
    [Route("email", Name = RouteNames.Email_Post, Order = 0)]
    public async Task<IActionResult> EmailAddress(EmailAddressViewModel model)
    {
        try
        {
            var response = await _mediator.Send(new CreateOtherStudentTriageDataCommand
            {
                StudentTriageData = new OtherStudentTriageData()
                {
                    Email = model.Email,
                    LepsCode = model.LepsCode
                }
            });

            var authenticateViewModel = new AuthenticateViewModel
            {
                AuthCode = response.AuthCode,
                ExpiryDate = response.ExpiryDate,
                StudentSurveyId = response.StudentSurveyId,
                LepsCode = model.LepsCode,
                Email = model.Email
            };

            TempData[TempDataKeys.TempDataAuthenticateModel] = JsonConvert.SerializeObject(authenticateViewModel);

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

