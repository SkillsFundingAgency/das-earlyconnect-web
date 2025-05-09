using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class SelectSchoolEditViewModel : TriageRouteModel
    {
        public int TotalCount { get; set; }
        public string SchoolSearchTerm { get; set; } = string.Empty;
        public string? SelectedSchool { get; set; }
        public string SelectedURN { get; set; } = string.Empty;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<EducationalOrganisations> EducationalOrganisations { get; set; } = new List<EducationalOrganisations>();
        public PaginationViewModel? PaginationViewModel { get; set; }
        public string BacklinkRoute => RouteNames.SearchSchool_Get;

        public Dictionary<string, string> RouteParameters
        {
            get
            {
                var routeDictionary = new Dictionary<string, string>
                {
                    {"studentSurveyId", StudentSurveyId.ToString()},
                    {"schoolSearchTerm", SchoolSearchTerm},
                    {"page", Page.ToString()},
                    {"pageSize", PageSize.ToString()},
                    {"isCheck", IsCheck.ToString()}
                };
                return routeDictionary;
            }
        }

        public static implicit operator SelectSchoolEditViewModel(GetEducationalOrganisationsResult request)
        {
            var selectSchoolEditViewModel = new SelectSchoolEditViewModel
            {
                EducationalOrganisations = request.EducationalOrganisations.Select(org => new EducationalOrganisations
                {
                    SchoolName = org.Name,
                    Address = FormatAddress(org.AddressLine1, org.Town, org.County, org.PostCode),
                    URN = org.URN
                }).ToList(),

                TotalCount = request.TotalCount,
            };

            return selectSchoolEditViewModel;
        }
        private static string FormatAddress(string addressLine1, string town, string county, string postCode)
        {
            var addressParts = new List<string>();

            if (!string.IsNullOrEmpty(addressLine1)) addressParts.Add(addressLine1);
            if (!string.IsNullOrEmpty(town)) addressParts.Add(town);
            if (!string.IsNullOrEmpty(county)) addressParts.Add(county);
            if (!string.IsNullOrEmpty(postCode)) addressParts.Add(postCode);

            return addressParts.Any() ? string.Join(", ", addressParts) : "Not recorded";
        }
    }

    public class EducationalOrganisations
    {
        public string SchoolName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string URN { get; set; } = string.Empty;
    }
}