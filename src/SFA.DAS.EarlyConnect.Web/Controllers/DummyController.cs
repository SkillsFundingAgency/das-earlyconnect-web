using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
namespace SFA.DAS.EarlyConnect.Web.Controllers;

[Authorize]
public class DummyController : Controller
{
    public DummyController()
    {

    }

    [HttpGet]
    [Route("dummy", Name = RouteNames.Dummy, Order = 0)]
    public IActionResult Dummy()
    {
        return View();
    }
}

