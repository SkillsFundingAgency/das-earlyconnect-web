using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class IndustryEditModel : TriageRouteModel
    {
        public List<string> Sector { get; set; } = new List<string>();
    }
}
