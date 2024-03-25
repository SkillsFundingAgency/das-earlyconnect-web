using System.Text;

namespace SFA.DAS.EarlyConnect.Domain.Interfaces
{
    public interface IJsonHelper
    {
        object LoadFromJSON(string path);
    }
}