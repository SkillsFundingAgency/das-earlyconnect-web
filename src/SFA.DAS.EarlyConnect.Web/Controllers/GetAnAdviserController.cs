using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Web.Infrastructure;


namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class GetAnAdviserController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetAnAdviserController> _logger;

    public GetAnAdviserController(IMediator mediator, ILogger<GetAnAdviserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public async Task<IActionResult> Index()
    {
        return View();
    }
}

