using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class TelephoneEditViewModel : TriageRouteModel
    {
        public string? Telephone { get; set; }
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswers_Get :
            IsOther ? RouteNames.ApprenticeshipLevel_Get :
            RouteNames.PersonalDetails_Get;
    }
}
