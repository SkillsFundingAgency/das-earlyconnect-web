namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class AuthCodeViewModel
    {
        public string? AuthCode { get; set; }
        public IList<string> OrderedFieldNames => new List<string>
        {
            nameof(AuthCode),
        };
    }
}