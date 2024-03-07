using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace SFA.DAS.EarlyConnect.Web.AppStart;
[ExcludeFromCodeCoverage]
public static class AddDataProtectionExtensions
{
    public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(EarlyConnect))
            .Get<EarlyConnect.Domain.Configuration.EarlyConnect>();

        if (config != null
            && !string.IsNullOrEmpty(config.DataProtectionKeysDatabase)
            && !string.IsNullOrEmpty(config.RedisConnectionString))
        {
            var redisConnectionString = config.RedisConnectionString;
            var dataProtectionKeysDatabase = config.DataProtectionKeysDatabase;

            var configurationOptions = StackExchange.Redis.ConfigurationOptions.Parse($"{redisConnectionString},{dataProtectionKeysDatabase}");
            var redis = ConnectionMultiplexer
                .Connect(configurationOptions);

            services.AddDataProtection()
                .SetApplicationName("das-earlyconnect")
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        }
    }
}