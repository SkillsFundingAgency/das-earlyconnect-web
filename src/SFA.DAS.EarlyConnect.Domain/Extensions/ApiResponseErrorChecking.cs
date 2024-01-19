using SFA.DAS.EarlyConnect.Domain.ApiResponse;
using SFA.DAS.EarlyConnect.Domain.Exceptions;
using System.Net;

namespace SFA.DAS.EarlyConnect.Domain.Extensions
{
    public static class ApiResponseErrorChecking
    {
        public static ApiResponse<T> EnsureSuccessStatusCode<T>(this ApiResponse<T> response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (!IsSuccessStatusCode(response.StatusCode))
            {
                throw ApiResponseException.Create(response);
            }

            return response;
        }

        public static bool IsSuccessStatusCode(HttpStatusCode statusCode)
            => (int)statusCode >= 200 && (int)statusCode <= 299;
    }
}