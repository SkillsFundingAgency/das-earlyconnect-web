using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Infrastructure.Api;
using SFA.DAS.EarlyConnect.Web.Services;

namespace SFA.DAS.EarlyConnect.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddTransient<IUrlValidator, UrlValidator>();
    }
}