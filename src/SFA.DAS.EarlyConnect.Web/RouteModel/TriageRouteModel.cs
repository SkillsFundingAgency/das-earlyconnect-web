﻿namespace SFA.DAS.EarlyConnect.Web.RouteModel
{
    public class TriageRouteModel
    {
        public Guid StudentSurveyId { get; set; }
        public bool IsCheck { get; set; }
        public bool IsOther { get; set; }
        public Dictionary<string, string> RouteDictionary
        {
            get
            {
                var routeDictionary = new Dictionary<string, string>
                {
                    {"studentSurveyId", StudentSurveyId.ToString()},
                    {"IsCheck", IsCheck.ToString()}
                };
                return routeDictionary;
            }
        }
    }
}