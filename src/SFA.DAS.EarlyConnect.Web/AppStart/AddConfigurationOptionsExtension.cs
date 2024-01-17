using Microsoft.Extensions.Options;
using SFA.DAS.EarlyConnect.Domain.Configuration;

namespace SFA.DAS.EarlyConnect.Web.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EarlyConnectOuterApi>(configuration.GetSection(nameof(EarlyConnectOuterApi)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EarlyConnectOuterApi>>().Value);
    }
}