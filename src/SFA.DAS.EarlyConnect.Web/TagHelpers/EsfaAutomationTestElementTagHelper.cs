using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.EarlyConnect.Web.Configuration;

namespace Esfa.Recruit.Shared.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = TagAttributeName)]
    public class EsfaAutomationTestElementTagHelper : TagHelper
    {
        private const string TagAttributeName = "esfa-automation";
        private const string DataAutomationAttributeName = "data-automation";
        private readonly IWebHostEnvironment _env;

        public EsfaAutomationTestElementTagHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HtmlAttributeName(TagAttributeName)]
        public string TargetName { get; set; }
        
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!_env.IsEnvironment(EnvironmentNames.PROD))
            {
                output.Attributes.SetAttribute(DataAutomationAttributeName, TargetName);
            }

            return Task.CompletedTask;
        }
    }
}