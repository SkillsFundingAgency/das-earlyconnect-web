using FluentValidation;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Infrastructure.Api;
using SFA.DAS.EarlyConnect.Web.Services;
using SFA.DAS.EarlyConnect.Web.Validations;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddTransient<IUrlValidator, UrlValidator>();
        services.AddTransient<IDataProtectorService, DataProtectorService>();
        services.AddSingleton<IValidator<AuthCodeViewModel>, AuthCodeModelValidator>();
        services.AddSingleton<IValidator<EmailAddressViewModel>, EmailAddressModelValidator>();
    }
}