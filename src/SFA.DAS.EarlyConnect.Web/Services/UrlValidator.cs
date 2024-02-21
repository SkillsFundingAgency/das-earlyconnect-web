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

            var properties = typeof(LepsRegionCodes).GetProperties();
            foreach (var property in properties)
            {
                if ((string)property.GetValue(_config.LepCodes) == lepsCode.Trim().ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsValidLinkDate(string date)
        {
            if (!_config.LinkValidityDays.HasValue)
            {
                return false;
            }

            return date.AsUKDateTime().Value.AddDays(_config.LinkValidityDays.Value) > DateTime.Now;
        }

        public bool DoesMatchAnyValue(LepsRegionCodes lepsRegionCodes, string valueToCheck)
        {
            var properties = typeof(LepsRegionCodes).GetProperties();
            foreach (var property in properties)
            {
                if ((string)property.GetValue(lepsRegionCodes) == valueToCheck)
                {
                    return true;
                }
            }
            return false;
        }
    }
}