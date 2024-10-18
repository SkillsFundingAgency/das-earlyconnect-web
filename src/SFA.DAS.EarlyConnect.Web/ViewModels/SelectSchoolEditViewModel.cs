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
        public List<EducationalOrganisations> EducationalOrganisations { get; set; } = new List<EducationalOrganisations>();
        public string BacklinkRoute => RouteNames.SearchSchool_Get;

        public static implicit operator SelectSchoolEditViewModel(GetEducationalOrganisationsResult request)
        {
            var selectSchoolEditViewModel = new SelectSchoolEditViewModel
            {
                EducationalOrganisations = request.EducationalOrganisations.Select(org => new EducationalOrganisations
                {
                    SchoolName = org.Name,
                    Address = $"{org.AddressLine1},{org.Town},{org.County},{org.PostCode}",
                    URN = org.URN
                }).ToList(),

                TotalCount = request.TotalCount,
            };

            return selectSchoolEditViewModel;
        }
    }

    public class EducationalOrganisations
    {
        public string SchoolName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string URN { get; set; } = string.Empty;
    }
}