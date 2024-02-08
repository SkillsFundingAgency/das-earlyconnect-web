namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class AuthCodeViewModel
    {
        public string? AuthCode { get; set; }
        public string? LepsCode { get; set; }
        public string? Email { get; set; }
        public bool ShowCodeResent { get; set; }
    }
}