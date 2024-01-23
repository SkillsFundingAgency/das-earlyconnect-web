using FluentValidation;
using Microsoft.Extensions.Options;
using SFA.DAS.EarlyConnect.Domain.Configuration;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Web.ViewModels.Validations;

namespace SFA.DAS.EarlyConnect.Web.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EarlyConnectWeb>(configuration.GetSection(nameof(EarlyConnectWeb)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EarlyConnectWeb>>().Value);
        services.Configure<EarlyConnectOuterApi>(configuration.GetSection(nameof(EarlyConnectOuterApi)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EarlyConnectOuterApi>>().Value);

        RegisterFluentValidators(services);
    }

    private static void RegisterFluentValidators(IServiceCollection services)
    {
        services.AddSingleton<IValidator<EmailAddressViewModel>, EmailAddressViewModelValidator>();
       
    }
}