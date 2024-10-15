using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class SearchSchoolEditViewModel : TriageRouteModel
    {
        [RegularExpression(@"^[\w\s]+$",
            ErrorMessage =
                "Enter the name of your school or college, or select 'I cannot find my school - enter manually'")]
        public string? SchoolSearchTerm { get; set; } = string.Empty;
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswers_Get :
            IsOther ? RouteNames.Industry_Get :
            RouteNames.Telephone_Get;
    }
}