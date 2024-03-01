using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Controllers;

public class PrivacyController : Controller
{    
    [HttpGet]
    [Route("privacy", Name = RouteNames.Privacy_Get, Order = 0)]
    public async Task<IActionResult> Privacy(SchoolNameViewModel m)
    {
        return View();
    }
}

