using System.Text.Json.Serialization;

namespace SFA.DAS.EarlyConnect.Domain.Interfaces;

public interface IPostApiRequest 
{
    [JsonIgnore]
    string PostUrl { get; }
    object Data { get; set; }
}