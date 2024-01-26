using Microsoft.AspNetCore.Mvc;
using MediatR;
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
    private readonly IAuthenticateService _authenticateService;

    public AuthenticateController(IMediator mediator,
        ILogger<AuthenticateController> logger,
        IUrlValidator urlValidator,
        IDataProtectorService dataProtectorService,
        IAuthenticateService authenticateService
        )
    {
        _mediator = mediator;
        _logger = logger;
        _urlValidator = urlValidator;
        _dataProtectorService = dataProtectorService;
        _authenticateService = authenticateService;
    }

    [HttpGet]
    [Route("authenticate", Name = RouteNames.Authenticate_Get, Order = 0)]
    public IActionResult Authenticate()
    {
        var viewModel = GetViewModelFromTempData();

        if (viewModel != null && _urlValidator.IsValidLepsCode(viewModel.LepsCode))
        {
            SetTempData(viewModel);

            return View(new AuthCodeViewModel { LepsCode = viewModel.LepsCode });
        }

        return NotFound();
    }

    [HttpPost]
    [Route("authenticate", Name = RouteNames.Authenticate_Post, Order = 0)]
    public async Task<IActionResult> Authenticate(AuthCodeViewModel request)
    {
        var viewModel = GetViewModelFromTempData();

        if (viewModel != null)
        {
            if (viewModel.AuthCode == null)
                return HandleAuthCodeError("Enter the correct confirmation code.", viewModel.LepsCode, viewModel);

            var decryptedAuthCode = _dataProtectorService.DecodeData(viewModel.AuthCode);

            if (request.AuthCode != decryptedAuthCode)
                return HandleAuthCodeError("Enter the correct confirmation code.", viewModel.LepsCode, viewModel);
            
            if (viewModel.ExpiryDate < DateTime.Now)
                return HandleAuthCodeError("The code you entered has expired. Enter the latest confirmation code.", viewModel.LepsCode, viewModel);

            await _authenticateService.SignInUser(viewModel.Email, viewModel.StudentSurveyId);

            return RedirectToRoute(RouteNames.Dummy, new { viewModel.StudentSurveyId });
        }

        return NotFound();
    }

    [HttpGet]
    [Route("send-code", Name = RouteNames.SendCode_Post, Order = 0)]
    public async Task<IActionResult> SendCode()
    {
        var viewModel = GetViewModelFromTempData();

        if (viewModel != null)
        {
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
                ExpiryDate = response.Expiry,
                LepsCode = viewModel.LepsCode,
                StudentSurveyId = viewModel.StudentSurveyId
            };

            SetTempData(authenticateViewModel);

            return RedirectToRoute(RouteNames.Authenticate_Get, new { viewModel.LepsCode });
        }

        return NotFound();
    }

    [HttpGet]
    [Route("emailaddress", Name = RouteNames.Email_Get, Order = 0)]
    public IActionResult EmailAddress(string? lepsCode)
    {
        var model = new EmailAddressViewModel
        {
            LepsCode = lepsCode
        };
        return View(model);
    }

    [HttpPost]
    [Route("emailaddress", Name = RouteNames.Email_Post, Order = 0)]
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
                ExpiryDate = response.Expiry,
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

    private void SetTempData(AuthenticateViewModel viewModel)
    {
        TempData[TempDataKeys.TempDataAuthenticateModel] = JsonConvert.SerializeObject(viewModel);
    }

    private AuthenticateViewModel GetViewModelFromTempData()
    {
        if (TempData[TempDataKeys.TempDataAuthenticateModel] is string model)
        {
            return JsonConvert.DeserializeObject<AuthenticateViewModel>(model);
        }

        return null;
    }

    private IActionResult HandleAuthCodeError(string errorMessage, string lepsCode, AuthenticateViewModel viewModel)
    {
        TempData[TempDataKeys.TempDataAuthenticateModel] = JsonConvert.SerializeObject(viewModel);
        ModelState.AddModelError(nameof(AuthCodeViewModel.AuthCode), errorMessage);
        return RedirectToRoute(RouteNames.Authenticate_Get, new { lepsCode });
    }
}

