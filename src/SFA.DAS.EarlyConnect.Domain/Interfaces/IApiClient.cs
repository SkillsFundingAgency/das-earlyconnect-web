using SFA.DAS.EarlyConnect.Domain.ApiResponse;

namespace SFA.DAS.EarlyConnect.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<ApiResponse<TResponse>> Post<TResponse>(IPostApiRequest request, bool includeResponse = true);
        Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request);
    }
}