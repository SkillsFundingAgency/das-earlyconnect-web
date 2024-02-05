using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers.SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Mappers
{
    public static class ManageStudentSurveyMapper
    {
        private static StudentTriageData MapFromRequest(BaseSurveyViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult, SurveyPage.Page page)
        {
            var manageStudentPersonalData = new StudentTriageData
            {
                Id = studentTriageDataBySurveyIdResult.Id,
                FirstName = studentTriageDataBySurveyIdResult.FirstName,
                LastName = studentTriageDataBySurveyIdResult.LastName,
                SchoolName = "ABC",
                DateOfBirth = studentTriageDataBySurveyIdResult.DateOfBirth,
                Email = studentTriageDataBySurveyIdResult.Email,
                Postcode = studentTriageDataBySurveyIdResult.Postcode,
                Telephone = studentTriageDataBySurveyIdResult.Telephone,
                DataSource = studentTriageDataBySurveyIdResult.DataSource,
                Industry = studentTriageDataBySurveyIdResult.Industry,
                StudentSurvey = studentTriageDataBySurveyIdResult.StudentSurvey
            };

            manageStudentPersonalData.StudentSurvey.ResponseAnswers ??= new List<ResponseAnswersDto>();

            var responseAnswersList = manageStudentPersonalData.StudentSurvey.ResponseAnswers.ToList();
            responseAnswersList.RemoveAll(answer => answer.QuestionId == (int)page);
            manageStudentPersonalData.StudentSurvey.ResponseAnswers = responseAnswersList;

            if (request.Question != null)
            {
                foreach (var selectedAnswers in request.Question.Answers.Where(answer => answer.IsSelected).ToList())
                {
                    var studentAnswer = new ResponseAnswersDto
                    {
                        QuestionId = (int)page,
                        StudentSurveyId = request.StudentSurveyId,
                        AnswerId = selectedAnswers.Id,
                        Response = "",
                        DateAdded = DateTime.Now
                    };

                    manageStudentPersonalData.StudentSurvey.ResponseAnswers.Add(studentAnswer);
                }
            }
            return manageStudentPersonalData;
        }

        public static StudentTriageData MapFromApprenticeshiplevelRequest(this ApprenticeshipLevelEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            return MapFromRequest(request, studentTriageDataBySurveyIdResult, SurveyPage.Page.Apprenticeshiplevel);
        }

        public static StudentTriageData MapFromAppliedForRequest(this AppliedForEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            return MapFromRequest(request, studentTriageDataBySurveyIdResult, SurveyPage.Page.AppliedFor);
        }

        public static StudentTriageData MapFromSupportRequest(this SupportEditViewModel request, GetStudentTriageDataBySurveyIdResult studentTriageDataBySurveyIdResult)
        {
            return MapFromRequest(request, studentTriageDataBySurveyIdResult, SurveyPage.Page.Support);
        }
    }
    namespace SFA.DAS.EarlyConnect.Web.ViewModels
    {
        public class BaseSurveyViewModel : TriageRouteModel
        {
            public Questions Question { get; set; }
        }
    }

}
