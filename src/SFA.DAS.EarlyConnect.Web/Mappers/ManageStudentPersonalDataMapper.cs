using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Extensions;
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
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.URN = studentTriageDataBySurveyIdResult.URN;
            manageStudentPersonalData.Postcode = request.PostalCode;
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Telephone;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.ResponseAnswers = new List<ResponseAnswersDto>();
            return manageStudentPersonalData;
        }
        public static StudentTriageData MapFromTelephoneRequest(this TelephoneEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            StudentTriageData manageStudentPersonalData = new StudentTriageData();
            manageStudentPersonalData.Id = studentTriageDataBySurveyIdResult.Id;
            manageStudentPersonalData.FirstName = studentTriageDataBySurveyIdResult.FirstName;
            manageStudentPersonalData.LastName = studentTriageDataBySurveyIdResult.LastName;
            manageStudentPersonalData.DateOfBirth = studentTriageDataBySurveyIdResult.DateOfBirth;
            manageStudentPersonalData.Email = studentTriageDataBySurveyIdResult.Email;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.URN = studentTriageDataBySurveyIdResult.URN;
            manageStudentPersonalData.Postcode = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.Telephone = request.Telephone ?? string.Empty;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.ResponseAnswers = new List<ResponseAnswersDto>();
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
            manageStudentPersonalData.URN = request.URN;
            manageStudentPersonalData.Email = studentTriageDataBySurveyIdResult.Email;
            manageStudentPersonalData.Postcode = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Telephone;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.ResponseAnswers = new List<ResponseAnswersDto>();
            return manageStudentPersonalData;
        }

        public static StudentTriageData MapFromNameRequest(this NameViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            StudentTriageData manageStudentPersonalData = new StudentTriageData();
            manageStudentPersonalData.Id = studentTriageDataBySurveyIdResult.Id;
            manageStudentPersonalData.FirstName = request.FirstName;
            manageStudentPersonalData.LastName = request.LastName;
            manageStudentPersonalData.DateOfBirth = studentTriageDataBySurveyIdResult.DateOfBirth;
            manageStudentPersonalData.Email = studentTriageDataBySurveyIdResult.Email;
            manageStudentPersonalData.Postcode = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Telephone;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.URN = studentTriageDataBySurveyIdResult.URN;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.ResponseAnswers = new List<ResponseAnswersDto>();
            return manageStudentPersonalData;
        }

        public static StudentTriageData MapFromDateOfBirthRequest(this DateOfBirthEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            StudentTriageData manageStudentPersonalData = new StudentTriageData();
            manageStudentPersonalData.Id = studentTriageDataBySurveyIdResult.Id;
            manageStudentPersonalData.FirstName = studentTriageDataBySurveyIdResult.FirstName;
            manageStudentPersonalData.LastName = studentTriageDataBySurveyIdResult.LastName;
            manageStudentPersonalData.DateOfBirth = request.DateOfBirth.AsUKDate();
            manageStudentPersonalData.Email = studentTriageDataBySurveyIdResult.Email;
            manageStudentPersonalData.Postcode = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Telephone;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.URN = studentTriageDataBySurveyIdResult.URN;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.ResponseAnswers = new List<ResponseAnswersDto>();
            return manageStudentPersonalData;
        }

        public static StudentTriageData MapFromIndustryRequest(this IndustryEditModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            StudentTriageData manageStudentPersonalData = new StudentTriageData();
            manageStudentPersonalData.Id = studentTriageDataBySurveyIdResult.Id;
            manageStudentPersonalData.FirstName = studentTriageDataBySurveyIdResult.FirstName;
            manageStudentPersonalData.LastName = studentTriageDataBySurveyIdResult.LastName;
            manageStudentPersonalData.DateOfBirth = studentTriageDataBySurveyIdResult.DateOfBirth;
            manageStudentPersonalData.Email = studentTriageDataBySurveyIdResult.Email;
            manageStudentPersonalData.Postcode = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Telephone;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = string.Join("|", request.Sector);
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.URN = studentTriageDataBySurveyIdResult.URN;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.LastUpdated = DateTime.Now;
            return manageStudentPersonalData;
        }

    }
}
