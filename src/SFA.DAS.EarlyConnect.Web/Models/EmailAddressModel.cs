using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Web.Models
{
    public class EmailAddressModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

    }
}
