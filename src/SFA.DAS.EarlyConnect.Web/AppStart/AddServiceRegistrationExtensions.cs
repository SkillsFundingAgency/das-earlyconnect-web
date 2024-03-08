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
        services.AddSingleton<IValidator<PostcodeEditViewModel>, PostcodeModelValidator>();
        services.AddSingleton<IValidator<TelephoneEditViewModel>, TelephoneModelValidator>();
        services.AddSingleton<IValidator<NameViewModel>, NameModelValidator>();
        services.AddSingleton<IValidator<DateOfBirthEditViewModel>, DateOfBirthModelValidator>();
        services.AddSingleton<IValidator<SchoolNameEditViewModel>, SchoolModelValidator>();
        services.AddSingleton<IValidator<RelocateEditViewModel>, RelocateModelValidator>();
        services.AddTransient<IAuthenticateService, AuthenticateService>();
        services.AddTransient<IJsonHelper, AreasOfInterestJsonHelper>();
    }
}