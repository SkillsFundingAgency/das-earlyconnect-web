using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using System.Net;

namespace SFA.DAS.EarlyConnect.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("accessdenied", Name = RouteNames.AccessDenied_Get, Order = 0)]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return View();
        }

        [HttpGet]
        [Route("formcompleted", Name = RouteNames.FormCompleted_Get, Order = 0)]
        public IActionResult FormCompleted()
        {
            return View();
        }
    }
}
