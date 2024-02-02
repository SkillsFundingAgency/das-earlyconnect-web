using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class SchoolNameEditViewModel : TriageRouteModel
    {
        [RegularExpression(@"^[\w\s&-]+$", ErrorMessage = "Invalid School Name")]
        public string? SchoolName { get; set; }
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswersDummy_Get :
            IsOther ? RouteNames.Industry_Get :
            RouteNames.Telephone_Get;
    }
}