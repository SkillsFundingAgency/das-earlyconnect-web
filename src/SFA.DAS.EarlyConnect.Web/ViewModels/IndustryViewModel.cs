namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class IndustryViewModel 
    {
        public List<string> Areas { get; set; } = new List<string>();
        public IList<string> OrderedFieldNames => new List<string>();
        public bool IsCheck { get; set; }
        public Guid StudentSurveyId { get; set; }
        public Dictionary<string, string> RouteDictionary
        {
            get
            {
                var routeDictionary = new Dictionary<string, string>
                {
                    {"studentSurveyId", StudentSurveyId.ToString()}
                };
                return routeDictionary;
            }
        }
    }
}