using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class IndustryViewModel : TriageRouteModel
    {
        public List<string> Areas { get; set; } = new List<string>();
        public IList<string> OrderedFieldNames => new List<string>();

        public List<string> Sector { get; set; } = new List<string>();

        public AreasOfInterestModel AreasOfInterest { get; set; }

        public class AreasOfInterestModel 
        {
            public List<AreaOfInterest> AreasOfInterest { get; set; }
        }
        public class AreaOfInterest
        {
            public string Area { get; set; }
            public List<Industry> Industry { get; set; }
        }
        public class Industry
        {
            public string Title { get; set; }
            public string Roles { get; set; }
        }

    }
}