using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class SelectSchoolViewModel : TriageRouteModel
    {
        public string SchoolSearchTerm { get; set; } = string.Empty;
    }
}