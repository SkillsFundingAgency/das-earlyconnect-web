using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class CheckYourAnswersViewModel : TriageRouteModel
    {
        public int Id { get; set; }
        public int? LepsId { get; set; }
        public int? LogId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? SchoolName { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? Postcode { get; set; }
        public string? DataSource { get; set; }
        public string? Industry { get; set; }
        public DateTime? DateInterest { get; set; }

        public List<Questions> Questions { get; set; } = new List<Questions>();

        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswersDummy_Get :
            IsOther ? RouteNames.Support_Get :
            RouteNames.Support_Get;


        public static implicit operator CheckYourAnswersViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var appliedForEditViewModel = new CheckYourAnswersViewModel();

            appliedForEditViewModel.Id = request.Id;
            appliedForEditViewModel.LepsId = request.LepsId;
            appliedForEditViewModel.LogId = request.LogId;
            appliedForEditViewModel.FirstName = request.FirstName;
            appliedForEditViewModel.LastName = request.LastName;
            appliedForEditViewModel.DateOfBirth = request.DateOfBirth;
            appliedForEditViewModel.SchoolName = request.SchoolName;
            appliedForEditViewModel.Telephone = request.Telephone;
            appliedForEditViewModel.Email = request.Email;
            appliedForEditViewModel.Postcode = request.Postcode;
            appliedForEditViewModel.DataSource = request.DataSource;
            appliedForEditViewModel.Industry = request.Industry;
            appliedForEditViewModel.Questions = request.SurveyQuestions.Select(c => (Questions)c).ToList();

            foreach (var surveyQuestion in appliedForEditViewModel.Questions)
            {
                if (surveyQuestion != null)
                {
                    var existingAnswers = surveyQuestion.Answers
                        .Where(answer => answer.QuestionId == surveyQuestion.Id)
                        .ToList();


                    surveyQuestion.Answers = new List<Answers>();

                    List<ResponseAnswersDto> matchingResponseAnswers = new List<ResponseAnswersDto>();

                    if (request.StudentSurvey?.ResponseAnswers != null)
                    {
                        matchingResponseAnswers = request.StudentSurvey.ResponseAnswers
                            .Where(answer => answer.QuestionId == surveyQuestion.Id)
                            .ToList();
                    }


                    foreach (var existingAnswer in existingAnswers)
                    {
                        var matchingAnswer =
                            matchingResponseAnswers.FirstOrDefault(answer => answer.AnswerId == existingAnswer.Id);

                        if (matchingAnswer != null)
                        {
                            var studentAnswer = existingAnswer;
                            surveyQuestion.Answers.Add(studentAnswer);
                        }
                    }
                }
            }

            return appliedForEditViewModel;
        }
    }
}