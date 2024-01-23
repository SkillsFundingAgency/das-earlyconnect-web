using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class EmailValidationViewModel
    {
        public string StudentSurveyId { get; set; }
        public string AuthCode { get; set; }
        public DateTime Expiry { get; set; }
        public string LepsCode { get; set; }
    }
}
