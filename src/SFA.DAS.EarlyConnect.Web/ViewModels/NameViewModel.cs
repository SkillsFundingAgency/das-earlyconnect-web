namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class NameViewModel
    {
        public Guid StudentSurveyId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IList<string> OrderedFieldNames => new List<string>
        {
            nameof(FirstName),
            nameof(LastName),
        };
        public bool IsCheck { get; set; }
    }
}