using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class EmailAddressViewModel
    {
        public string Email { get; set; }
        public string LepsCode { get; set; } = "E37000051";

        public IList<string> OrderedFieldNames => new List<string>
        {
            nameof(Email )
        };

    }
}
