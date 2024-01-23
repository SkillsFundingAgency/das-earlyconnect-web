﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using System.Net;

namespace Esfa.Recruit.Employer.Web.Controllers
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
    }
}