using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;

namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class GetAnAdviserController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetAnAdviserController> _logger;
    private readonly IUrlValidator _urlValidator;

    public GetAnAdviserController(IMediator mediator,
        ILogger<GetAnAdviserController> logger,
        IUrlValidator urlValidator
        )
    {
        _mediator = mediator;
        _logger = logger;
        _urlValidator = urlValidator;
    }

    [HttpGet]
    [Route("{lepsCode}", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public IActionResult Index(string lepsCode)
    {
        return _urlValidator.IsValidLepsCode(lepsCode) ? View() : NotFound();
    }

    [HttpPost]
    [Route("{lepsCode}", Name = RouteNames.GetAnAdviser_Post, Order = 0)]
    public IActionResult GetAnAdviser_Post(string lepsCode)
    {
        return RedirectToRoute(RouteNames.Email_Get, new { lepsCode = lepsCode });

    }

    [HttpGet]
    [Route("dummy", Name = RouteNames.Dummy)]
    public IActionResult Dummy(string lepsCode)
    {
        return View();
    }
}

