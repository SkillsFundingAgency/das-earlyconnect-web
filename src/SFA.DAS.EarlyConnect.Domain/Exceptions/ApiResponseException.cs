using System.Net;
using SFA.DAS.EarlyConnect.Domain.ApiResponse;

namespace SFA.DAS.EarlyConnect.Domain.Exceptions
{
    public class ApiResponseException : Exception
    {
        public HttpStatusCode Status { get; }
        public string Error { get; }

        public static ApiResponseException Create<T>(ApiResponse<T> response)
        {
            return new ApiResponseException(response.StatusCode, response.ErrorContent);
        }

        public ApiResponseException(HttpStatusCode status, string error)
            : base($"HTTP status code did not indicate success: {(int)status} {status}")
        {
            Status = status;
            Error = error;
        }

        public ApiResponseException(HttpStatusCode status, string error, Exception innerException)
            : base($"HTTP status code did not indicate success: {(int)status} {status}", innerException)
        {
            Status = status;
            Error = error;
        }
    }
}