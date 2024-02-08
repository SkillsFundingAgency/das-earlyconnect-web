namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class AuthenticateViewModel 
    {
        public string StudentSurveyId { get; set; }
        public string AuthCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string LepsCode { get; set; }
        public string Email { get; set; }
        public bool ShowCodeResent { get; set; }
    }
}