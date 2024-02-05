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
            manageStudentPersonalData.Telephone = studentTriageDataBySurveyIdResult.Telephone;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.LastUpdated = DateTime.Now;
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
            manageStudentPersonalData.Postcode = studentTriageDataBySurveyIdResult.Postcode;
            manageStudentPersonalData.Telephone = request.Telephone;
            manageStudentPersonalData.DataSource = studentTriageDataBySurveyIdResult.DataSource;
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.LastUpdated = DateTime.Now;
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
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.LastUpdated = DateTime.Now;
            return manageStudentPersonalData;
        }

        public static StudentTriageData MapFromMoveRequest(this MoveEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
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
            manageStudentPersonalData.Industry = studentTriageDataBySurveyIdResult.Industry;
            manageStudentPersonalData.SchoolName = studentTriageDataBySurveyIdResult.SchoolName;


            var previousAnswers = studentTriageDataBySurveyIdResult.StudentSurvey.ResponseAnswers.Where(a => a.QuestionId == request.QuestionId).ToList();
            foreach (var answer in previousAnswers)
            {
                studentTriageDataBySurveyIdResult.StudentSurvey.ResponseAnswers.Remove(answer);
            }

            foreach (var selectedAnswer in request.Relocate)
            {
                studentTriageDataBySurveyIdResult.StudentSurvey.ResponseAnswers.Add(
                    new Domain.GetStudentTriageDataBySurveyId.ResponseAnswersDto 
                    { 
                        AnswerId = int.Parse(selectedAnswer),
                        DateAdded = DateTime.Now,
                        QuestionId = request.QuestionId.Value,
                        StudentSurveyId = request.StudentSurveyId,
                        Response = string.Empty
                    });
            }
            manageStudentPersonalData.StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey;
            manageStudentPersonalData.StudentSurvey.LastUpdated = DateTime.Now;

            return manageStudentPersonalData;
        }
    }
}
