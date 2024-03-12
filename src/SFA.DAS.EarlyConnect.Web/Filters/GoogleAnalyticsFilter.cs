using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.EarlyConnect.Web.Infrastructure;

namespace Esfa.Recruit.Employer.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        private readonly ILogger<GoogleAnalyticsFilter> _logger;

        public GoogleAnalyticsFilter(ILogger<GoogleAnalyticsFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var controller = filterContext.Controller as Controller;
                if (controller?.User != null)
                {
                    var surveyIdFromUrl = controller.HttpContext.Request.Query[RouteValues.SurveyId];
                    controller.ViewBag.GaData = new GaData
                    {
                        SurveyId = surveyIdFromUrl,
                        //Acc = accountIdFromUrl
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GoogleAnalyticsFilter Cannot set GaData for Employer");
            }
            
            base.OnActionExecuting(filterContext);
        }

        public class GaData
        {
            public string SurveyId { get; set; }
            public string Vpv { get; set; }
            public string Acc { get; set; }
        }
    }
}
