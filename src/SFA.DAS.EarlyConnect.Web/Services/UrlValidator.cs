using SFA.DAS.EarlyConnect.Domain.Configuration;
using Microsoft.Extensions.Options;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Extensions;

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
            if (_config.LepCodes == null)
            {
                return false;
            }

            return _config.LepCodes.Split(',').Contains(lepsCode.Trim().ToUpper());
        }

        public bool IsValidLinkDate(string date)
        {
            if (!_config.LinkValidityDays.HasValue)
            {
                return false;
            }

            return date.AsUKDateTime().Value.AddDays(_config.LinkValidityDays.Value) > DateTime.Now;
        }
    }
}