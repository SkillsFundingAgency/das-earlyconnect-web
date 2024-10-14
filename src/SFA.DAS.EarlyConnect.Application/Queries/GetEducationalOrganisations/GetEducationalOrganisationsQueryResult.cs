using SFA.DAS.EarlyConnect.Domain.GetEducationalOrganisations;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations
{
    public class GetEducationalOrganisationsResult
    {
        public ICollection<GetEducationalOrganisationsResponse>? EducationalOrganisations { get; set; }
    }
}