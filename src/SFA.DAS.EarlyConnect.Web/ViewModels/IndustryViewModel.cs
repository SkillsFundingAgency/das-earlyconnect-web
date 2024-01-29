namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class IndustryViewModel
    {
        public string? StudentSurveyId { get; set; }
        public List<string> Areas { get; set; } = new List<string>();
        public IList<string> OrderedFieldNames => new List<string>();
        public bool IsCheck { get; set; }
    }
}