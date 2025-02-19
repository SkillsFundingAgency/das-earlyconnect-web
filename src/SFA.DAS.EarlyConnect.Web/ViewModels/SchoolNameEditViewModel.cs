using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class SchoolNameEditViewModel : TriageRouteModel
    {
        [RegularExpression(@"^[\w\s&-']+$", ErrorMessage = "Invalid School Name")]
        public string? SchoolName { get; set; }
        public string URN { get; set; } = string.Empty;
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswers_Get :
            IsOther ? RouteNames.SearchSchool_Get :
            RouteNames.SearchSchool_Get;
    }
}