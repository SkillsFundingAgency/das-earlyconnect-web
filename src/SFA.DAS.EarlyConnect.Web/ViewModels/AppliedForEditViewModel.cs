using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers.SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class AppliedForEditViewModel : BaseSurveyViewModel
    {
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswersDummy_Get :
            IsOther ? RouteNames.ApprenticeshipLevel_Get :
            RouteNames.ApprenticeshipLevel_Get;

        public static implicit operator AppliedForEditViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var appliedForEditViewModel = new AppliedForEditViewModel();

            var surveyQuestion = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.AppliedFor);

            if (surveyQuestion != null)
            {
                appliedForEditViewModel.Question = new Questions();
                appliedForEditViewModel.Question.Id = surveyQuestion.Id;
                appliedForEditViewModel.Question.SurveyId = surveyQuestion.SurveyId;
                appliedForEditViewModel.Question.QuestionTypeId = surveyQuestion.QuestionTypeId;
                appliedForEditViewModel.Question.QuestionText = surveyQuestion.QuestionText;
                appliedForEditViewModel.Question.ShortDescription = surveyQuestion.ShortDescription;
                appliedForEditViewModel.Question.SummaryLabel = surveyQuestion.SummaryLabel;
                appliedForEditViewModel.Question.ValidationMessage = surveyQuestion.ValidationMessage;
                appliedForEditViewModel.Question.DefaultToggleAnswerId = surveyQuestion.DefaultToggleAnswerId;
                appliedForEditViewModel.Question.SortOrder = surveyQuestion.SortOrder;
                appliedForEditViewModel.Question.Answers = new List<Answers>();

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
                    appliedForEditViewModel.Question.Answers.Add(studentAnswer);
                }
            }

            return appliedForEditViewModel;
        }
    }
}