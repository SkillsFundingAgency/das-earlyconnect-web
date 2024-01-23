using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Models;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.Interfaces;


namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class EmailAddressController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmailAddressController> _logger;
    private readonly IApiClient _apiClient;

    public EmailAddressController(IMediator mediator, ILogger<EmailAddressController> logger, IApiClient apiClient)
    {
        _mediator = mediator;
        _logger = logger;
        _apiClient = apiClient;
    }

    [HttpGet]
    [Route("emailaddress/{lepsCode}", Name = RouteNames.EmailAddress_Get, Order = 0)]
    public IActionResult Index()
    {
        return View();

    }

    [HttpPost]
    [Route("emailaddress/{lepsCode}", Name = RouteNames.EmailAddress_Post, Order = 0)]
    public async Task<IActionResult> EmailAddressPost(string lepsCode, EmailAddressModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var command = new CreateOtherStudentTriageDataCommand { 
            StudentTriageData = new OtherStudentTriageData() 
            { 
                Email = model.Email,
                LepsCode = lepsCode
            } 
        };

        var handler = new CreateOtherStudentTriageDataCommandHandler(_apiClient);
        var commandResult = await handler.Handle(command, CancellationToken.None);

        return RedirectToRoute(RouteNames.Dummy);

    }
}

