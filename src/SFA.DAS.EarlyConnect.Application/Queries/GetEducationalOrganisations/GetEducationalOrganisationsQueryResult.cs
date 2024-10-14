using SFA.DAS.EarlyConnect.Domain.GetEducationalOrganisations;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations
{
    public class GetEducationalOrganisationsResult
    {
        public int TotalCount { get; set; }
        public ICollection<EducationalOrganisationData>? EducationalOrganisations { get; set; }
    }
}