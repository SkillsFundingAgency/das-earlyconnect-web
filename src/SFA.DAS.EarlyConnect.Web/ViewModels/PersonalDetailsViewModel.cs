using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class PersonalDetailsViewModel : TriageRouteModel
    {
        public int Id { get; set; }
        public int? LepsId { get; set; }
        public int? LogId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Postcode { get; set; }
        public string? DataSource { get; set; }
        public string? Industry { get; set; }

        public string? BacklinkRoute => RouteNames.UCASServiceStart_Get;



        public static implicit operator PersonalDetailsViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var personalDetailsViewModel = new PersonalDetailsViewModel();

            personalDetailsViewModel.StudentSurveyId = request.StudentSurvey.Id;
            personalDetailsViewModel.Id = request.Id;
            personalDetailsViewModel.LepsId = request.LepsId;
            personalDetailsViewModel.LogId = request.LogId;
            personalDetailsViewModel.FirstName = request.FirstName;
            personalDetailsViewModel.LastName = request.LastName;
            personalDetailsViewModel.DateOfBirth = request.DateOfBirth;
            personalDetailsViewModel.Email = request.Email;
            personalDetailsViewModel.Postcode = request.Postcode;
            personalDetailsViewModel.DataSource = request.DataSource;
            personalDetailsViewModel.Industry = request.Industry;
            return personalDetailsViewModel;
        }
    }
}