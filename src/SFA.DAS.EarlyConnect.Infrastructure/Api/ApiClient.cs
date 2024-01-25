using Microsoft.Extensions.Options;
using SFA.DAS.EarlyConnect.Domain.Configuration;
using System.Net;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using SFA.DAS.EarlyConnect.Domain.ApiResponse;

namespace SFA.DAS.EarlyConnect.Infrastructure.Api;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly EarlyConnectOuterApi _config;

    public ApiClient(HttpClient httpClient, IOptions<EarlyConnectOuterApi> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
        _httpClient.BaseAddress = new Uri(config.Value.ApiBaseUrl);
    }

    public async Task<ApiResponse<TResponse>> Post<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        var stringContent = request.Data != null ? new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, "application/json") : null;

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl);

        requestMessage.Content = stringContent;

        AddAuthenticationHeader(requestMessage);

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var errorContent = "";
        var responseBody = (TResponse)default;

        if (response.StatusCode.Equals(HttpStatusCode.NotFound))
        {
            errorContent = json;
            HandleException(response, json);
        }
        else if (includeResponse)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
        }

        var postWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

        return postWithResponseCode;
    }

    public async Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);

        AddAuthenticationHeader(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var errorContent = "";
        var responseBody = (TResponse)default;

        if (IsNot200RangeResponseCode(response.StatusCode))
        {
            errorContent = json;
        }
        else if (string.IsNullOrWhiteSpace(json))
        {
            // 204 No Content from a potential returned null
            // Will throw if attempts to deserialise but didn't
            // feel right making it part of the error if branch
            // even if there is no content.
        }
        else
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
        }

        var getWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

        return getWithResponseCode;
    }

    public virtual string HandleException(HttpResponseMessage response, string json)
    {
        return json;
    }
    private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
    {
        return !((int)statusCode >= 200 && (int)statusCode <= 299);
    }

    private void AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _config.SubscriptionKey);
        httpRequestMessage.Headers.Add("X-Version", "1");
    }
}