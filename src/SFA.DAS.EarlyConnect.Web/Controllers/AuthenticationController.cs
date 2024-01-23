using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Web.Configuration;
using SFA.DAS.EarlyConnect.Web.ViewModels;


namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class AuthenticationController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IMediator mediator, ILogger<AuthenticationController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("authentication/email/{lepsCode}", Name = RouteNames.EmailAddress_Get, Order = 0)]
    public IActionResult Index()
    {
        return View(new EmailAddressViewModel());

    }

    [HttpPost]
    [Route("authentication/email/{lepsCode}", Name = RouteNames.EmailAddress_Post, Order = 0)]
    public async Task<IActionResult> EmailAddressPost(EmailAddressViewModel model, string lepsCode)
    {
        //string lepsCode = "E37000051";
        try 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _mediator.Send(new CreateOtherStudentTriageDataCommand
            {
                StudentTriageData = new OtherStudentTriageData()
                {
                    Email = model.Email,
                    LepsCode = lepsCode
                }
            });

            TempData.Add(TempDataKeys.EmailAuthenticationCode, new EmailValidationViewModel
            { 
                AuthCode = response.AuthCode,
                Expiry = response.ExpiryDate,
                StudentSurveyId = response.StudentSurveyId,
                LepsCode = lepsCode
            });

            return RedirectToRoute(RouteNames.Dummy);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error posting email address");
            return BadRequest();
        }
    }
}

