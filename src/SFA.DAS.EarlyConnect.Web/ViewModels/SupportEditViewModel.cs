using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers.SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class SupportEditViewModel : BaseSurveyViewModel
    {
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswersDummy_Get :
            IsOther ? RouteNames.AppliedFor_Get :
            RouteNames.AppliedFor_Get;

        public static implicit operator SupportEditViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var supportEditViewModel = new SupportEditViewModel();

            var surveyQuestion = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Support);

            if (surveyQuestion != null)
            {
                supportEditViewModel.Question = new Questions();
                supportEditViewModel.Question.Id = surveyQuestion.Id;
                supportEditViewModel.Question.SurveyId = surveyQuestion.SurveyId;
                supportEditViewModel.Question.QuestionTypeId = surveyQuestion.QuestionTypeId;
                supportEditViewModel.Question.QuestionText = surveyQuestion.QuestionText;
                supportEditViewModel.Question.ShortDescription = surveyQuestion.ShortDescription;
                supportEditViewModel.Question.SummaryLabel = surveyQuestion.SummaryLabel;
                supportEditViewModel.Question.ValidationMessage = surveyQuestion.ValidationMessage;
                supportEditViewModel.Question.DefaultToggleAnswerId = surveyQuestion.DefaultToggleAnswerId;
                supportEditViewModel.Question.SortOrder = surveyQuestion.SortOrder;
                supportEditViewModel.Question.Answers = new List<Answers>();

                var existingAnswers = surveyQuestion.Answers
                    .Where(answer => answer.QuestionId == surveyQuestion.Id)
                    .ToList();

                List<ResponseAnswersDto> matchingResponseAnswers = new List<ResponseAnswersDto>();

                if (request.StudentSurvey?.ResponseAnswers != null)
                {
                    matchingResponseAnswers = request.StudentSurvey.ResponseAnswers
                        .Where(answer => answer.QuestionId == surveyQuestion.Id)
                        .ToList();
                }

                int serialCounter = 0;

                foreach (var existingAnswer in existingAnswers)
                {
                    var matchingAnswer = matchingResponseAnswers.FirstOrDefault(answer => answer.AnswerId == existingAnswer.Id);

                    var studentAnswer = new Answers();

                    if (matchingAnswer != null)
                    {
                        studentAnswer.IsSelected = true;
                        studentAnswer.StudentAnswerId = matchingAnswer.Id;
                    }

                    studentAnswer.Id = existingAnswer.Id;
                    studentAnswer.QuestionId = existingAnswer.QuestionId;
                    studentAnswer.AnswerText = existingAnswer.AnswerText;
                    studentAnswer.ShortDescription = existingAnswer.ShortDescription;
                    studentAnswer.GroupNumber = existingAnswer.GroupNumber;
                    studentAnswer.GroupLabel = existingAnswer.GroupLabel;
                    studentAnswer.SortOrder = existingAnswer.SortOrder;

                    studentAnswer.Serial = serialCounter;
                    serialCounter++;

                    studentAnswer.ToggleAnswer = existingAnswer.Id == surveyQuestion.DefaultToggleAnswerId
                        ? AnswerGroup.Group.Toggled
                        : AnswerGroup.Group.Default;
                    supportEditViewModel.Question.Answers.Add(studentAnswer);
                }
            }

            return supportEditViewModel;
        }
    }
}