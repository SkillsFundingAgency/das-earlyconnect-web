using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class ValidationSummaryViewModel
    {
        public IList<string> OrderedFieldNames { get; set; } = new List<string>();
        public ModelStateDictionary ModelState { get; set; }
    }
}
