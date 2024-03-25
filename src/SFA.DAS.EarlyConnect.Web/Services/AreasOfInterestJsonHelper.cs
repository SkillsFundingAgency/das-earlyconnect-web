using SFA.DAS.EarlyConnect.Domain.Interfaces;
using System.Text.Json;
using static SFA.DAS.EarlyConnect.Web.ViewModels.IndustryViewModel;

namespace SFA.DAS.EarlyConnect.Web.Services
{
    public class AreasOfInterestJsonHelper : IJsonHelper
    {
        public object LoadFromJSON(string path)
        {
            string jsonContent = System.IO.File.ReadAllText(path);
            return JsonSerializer.Deserialize<AreasOfInterestModel>(jsonContent);
        }
    }
}