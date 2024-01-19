using System.Text.Json.Serialization;

namespace SFA.DAS.EarlyConnect.Domain.Interfaces;

public interface IGetApiRequest
{
    [JsonIgnore]
    string GetUrl { get;}
}