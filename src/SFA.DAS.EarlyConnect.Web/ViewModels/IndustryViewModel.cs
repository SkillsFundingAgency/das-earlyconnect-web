using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class IndustryViewModel : TriageRouteModel
    {
        public List<string> Areas { get; set; } = new List<string>();
        public IList<string> OrderedFieldNames => new List<string>();

        public List<string> Sector { get; set; } = new List<string>();
    }
}