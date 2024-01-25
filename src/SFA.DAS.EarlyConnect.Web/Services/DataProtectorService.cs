using System.Text;

namespace SFA.DAS.EarlyConnect.Application.Services
{
    public interface IDataProtectorService
    {
        string EncodedData(string authCode);
        string DecodeData(string encryptedAuthCode);
    }

    public class DataProtectorService : IDataProtectorService
    {
        public string EncodedData(string authCode)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(authCode));
        }

        public string DecodeData(string encryptedAuthCode)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedAuthCode));
        }
    }
}
