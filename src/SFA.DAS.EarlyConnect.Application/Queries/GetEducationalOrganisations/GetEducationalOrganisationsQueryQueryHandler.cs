using MediatR;
using SFA.DAS.EarlyConnect.Domain.Extensions;
using SFA.DAS.EarlyConnect.Domain.GetEducationalOrganisations;
using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations
{
    public class GetEducationalOrganisationsQueryHandler : IRequestHandler<GetEducationalOrganisationsQuery, GetEducationalOrganisationsResult>
    {
        private readonly IApiClient _apiClient;

        public GetEducationalOrganisationsQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetEducationalOrganisationsResult> Handle(GetEducationalOrganisationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetEducationalOrganisationsResponse>(new GetEducationalOrganisationsRequest(request.LepCode, request.SearchTerm, request.Page, request.PageSize));

            result.EnsureSuccessStatusCode();

            return new GetEducationalOrganisationsResult
            {
                TotalCount = result.Body.TotalCount,
                EducationalOrganisations = result.Body.EducationalOrganisations
            };
        }
    }
}