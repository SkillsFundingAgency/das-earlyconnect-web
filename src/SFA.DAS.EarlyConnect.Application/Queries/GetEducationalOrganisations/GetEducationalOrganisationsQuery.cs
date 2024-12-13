using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations
{
    public class GetEducationalOrganisationsQuery : IRequest<GetEducationalOrganisationsResult>
    {
        public string LepCode { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}