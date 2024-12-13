using SFA.DAS.EarlyConnect.Domain.Interfaces;
using System.Web;

namespace SFA.DAS.EarlyConnect.Domain.GetEducationalOrganisations
{
    public class GetEducationalOrganisationsRequest : IGetApiRequest
    {
        public string LepCode { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetEducationalOrganisationsRequest(string lepCode, string? searchTerm, int page, int pageSize)
        {
            LepCode = lepCode;
            SearchTerm = searchTerm;
            Page = page;
            PageSize = pageSize;
        }

        public string GetUrl => $"educational-organisations-data?LepCode={HttpUtility.UrlEncode(LepCode)}&SearchTerm={HttpUtility.UrlEncode(SearchTerm)}&Page={Page}&PageSize={PageSize}";
    }
}