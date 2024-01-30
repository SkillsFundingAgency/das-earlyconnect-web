using Esfa.Recruit.Employer.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class NameViewModel : TriageRouteModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IList<string> OrderedFieldNames => new List<string>
        {
            nameof(FirstName),
            nameof(LastName),
        };
    }
}