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
            IsOther ? RouteNames.Relocate_Get :
            RouteNames.Relocate_Get;

        public static implicit operator SupportEditViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var supportEditViewModel = new SupportEditViewModel
            {
                Question = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Support)
            };

            var surveyQuestion = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Support);

            if (surveyQuestion != null)
            {
                var existingAnswers = supportEditViewModel.Question.Answers
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

                    if (matchingAnswer != null)
                    {
                        existingAnswer.IsSelected = true;
                        existingAnswer.StudentAnswerId = matchingAnswer.Id;
                    }

                    existingAnswer.Serial = serialCounter;
                    serialCounter++;

                    existingAnswer.ToggleAnswer = existingAnswer.Id == surveyQuestion.DefaultToggleAnswerId
                        ? AnswerGroup.Group.Toggled
                        : AnswerGroup.Group.Default;
                }

                supportEditViewModel.Question.Answers = existingAnswers;
            }

            return supportEditViewModel;
        }
    }
}