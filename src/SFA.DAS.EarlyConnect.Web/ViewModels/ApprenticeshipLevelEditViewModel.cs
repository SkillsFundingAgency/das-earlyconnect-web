using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers.SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class ApprenticeshipLevelEditViewModel : BaseSurveyViewModel
    {
        public string? BacklinkRoute =>
            IsCheck && IsOther ? RouteNames.CheckYourAnswers_Get :
            IsCheck && !IsOther ? RouteNames.CheckYourAnswersDummy_Get :
            IsOther ? RouteNames.SchoolName_Get :
            RouteNames.SchoolName_Get;

        public static implicit operator ApprenticeshipLevelEditViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var apprenticeshipLevelEditViewModel = new ApprenticeshipLevelEditViewModel();

            var surveyQuestion = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Apprenticeshiplevel);

            if (surveyQuestion != null)
            {
                apprenticeshipLevelEditViewModel.Question = new Questions();
                apprenticeshipLevelEditViewModel.Question.Id = surveyQuestion.Id;
                apprenticeshipLevelEditViewModel.Question.SurveyId = surveyQuestion.SurveyId;
                apprenticeshipLevelEditViewModel.Question.QuestionTypeId = surveyQuestion.QuestionTypeId;
                apprenticeshipLevelEditViewModel.Question.QuestionText = surveyQuestion.QuestionText;
                apprenticeshipLevelEditViewModel.Question.ShortDescription = surveyQuestion.ShortDescription;
                apprenticeshipLevelEditViewModel.Question.SummaryLabel = surveyQuestion.SummaryLabel;
                apprenticeshipLevelEditViewModel.Question.ValidationMessage = surveyQuestion.ValidationMessage;
                apprenticeshipLevelEditViewModel.Question.DefaultToggleAnswerId = surveyQuestion.DefaultToggleAnswerId;
                apprenticeshipLevelEditViewModel.Question.SortOrder = surveyQuestion.SortOrder;
                apprenticeshipLevelEditViewModel.Question.Answers = new List<Answers>();

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
                    apprenticeshipLevelEditViewModel.Question.Answers.Add(studentAnswer);
                }
            }

            return apprenticeshipLevelEditViewModel;
        }
    }
}