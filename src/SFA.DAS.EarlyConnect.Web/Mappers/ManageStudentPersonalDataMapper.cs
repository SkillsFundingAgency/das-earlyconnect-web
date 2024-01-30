using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Mappers
{
    public static class ManageStudentPersonalDataMapper
    {
        public static StudentTriageData MapFromPostcodeRequest(this PostcodeEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            StudentTriageData manageStudentPersonalData = new StudentTriageData();
            manageStudentPersonalData.Id = studentTriageDataBySurveyIdResult.Id;
            manageStudentPersonalData.FirstName = studentTriageDataBySurveyIdResult.FirstName;
            manageStudentPersonalData.LastName = studentTriageDataBySurveyIdResult.LastName;
            manageStudentPersonalData.DateOfBirth = studentTriageDataBySurveyIdResult.DateOfBirth;
            manageStudentPersonalData.Email = studentTriageDataBySurveyIdResult.Email;
            manageStudentPersonalData.Postcode = request.Postcode;
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.LastUpdated = DateTime.Now;
            return manageStudentPersonalData;
        }
        public static StudentTriageData MapFromSchoolNameRequest(this SchoolNameEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            StudentTriageData manageStudentPersonalData = new StudentTriageData();
            manageStudentPersonalData.Id = studentTriageDataBySurveyIdResult.Id;
            manageStudentPersonalData.FirstName = studentTriageDataBySurveyIdResult.FirstName;
            manageStudentPersonalData.LastName = studentTriageDataBySurveyIdResult.LastName;
            manageStudentPersonalData.DateOfBirth = studentTriageDataBySurveyIdResult.DateOfBirth;
            manageStudentPersonalData.SchoolName = request.SchoolName;
            manageStudentPersonalData.Email = studentTriageDataBySurveyIdResult.Email;
            manageStudentPersonalData.Postcode = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.LastUpdated = DateTime.Now;
            return manageStudentPersonalData;
        }
    }
}
