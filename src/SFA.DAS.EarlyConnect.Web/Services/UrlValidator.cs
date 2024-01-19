using SFA.DAS.EarlyConnect.Domain.Configuration;
using Microsoft.Extensions.Options;
using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Web.Services
{
    public class UrlValidator : IUrlValidator
    {
        private EarlyConnectWeb _config;

        public UrlValidator(IOptions<EarlyConnectWeb> config)
        {
            _config = config.Value;
        }

        public bool IsValidLepsCode(string lepsCode)
        {
            return _config.LepCodes.Split(',').Contains(lepsCode.Trim().ToUpper());
        }
    }
}