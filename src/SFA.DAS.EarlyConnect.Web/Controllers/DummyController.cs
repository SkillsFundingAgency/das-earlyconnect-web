using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
namespace SFA.DAS.EarlyConnect.Web.Controllers;

[Authorize]
public class DUmmyController : Controller
{
    public DUmmyController()
    {

    }

    [HttpGet]
    [Route("dummy", Name = RouteNames.Dummy, Order = 0)]
    public IActionResult Dummy()
    {
        return View();
    }
}

