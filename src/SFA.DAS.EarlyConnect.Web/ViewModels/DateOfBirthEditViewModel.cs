using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class DateOfBirthEditViewModel : TriageRouteModel
    {
        public string? Day { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }

        public string? DateOfBirth
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Day) ||
                    string.IsNullOrWhiteSpace(Month) ||
                    string.IsNullOrWhiteSpace(Year))
                {
                    return null;
                }
                return $"{Day}/{Month}/{Year}";
            }
        }
    }
}