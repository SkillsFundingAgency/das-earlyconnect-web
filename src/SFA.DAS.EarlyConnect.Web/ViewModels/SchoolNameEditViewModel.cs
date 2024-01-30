using Esfa.Recruit.Employer.Web.RouteModel;
using SFA.DAS.EarlyConnect.Web.Infrastructure;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class SchoolNameEditViewModel : TriageRouteModel
    {
        public string? SchoolName { get; set; }
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswersDummy_Get :
            IsOther ? RouteNames.Industry_Get :
            RouteNames.Telephone_Get;
    }
}