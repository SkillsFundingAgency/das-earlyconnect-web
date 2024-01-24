namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class EmailAddressViewModel
    {
        public string? Email { get; set; }
        public string? LepsCode { get; set; }
        public IList<string> OrderedFieldNames => new List<string>
        {
            nameof(Email),
        };
    }
}