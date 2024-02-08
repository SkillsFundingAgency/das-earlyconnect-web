using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers.SFA.DAS.EarlyConnect.Web.ViewModels;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class RelocateEditViewModel : BaseSurveyViewModel
    {
        public int SelectedAnswerId { get; set; }

        public static implicit operator RelocateEditViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var relocateEditViewModel = new RelocateEditViewModel
            {
                Question = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Relocate)
            };

            var surveyQuestion = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Relocate);

            if (surveyQuestion != null)
            {
                var existingAnswers = relocateEditViewModel.Question.Answers
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
                        relocateEditViewModel.SelectedAnswerId = matchingAnswer.AnswerId.GetValueOrDefault();
                    }

                    existingAnswer.Serial = serialCounter;
                    serialCounter++;

                    existingAnswer.ToggleAnswer = existingAnswer.Id == surveyQuestion.DefaultToggleAnswerId
                        ? AnswerGroup.Group.Toggled
                        : AnswerGroup.Group.Default;
                }

                relocateEditViewModel.Question.Answers = existingAnswers;
            }

            return relocateEditViewModel;
        }
    }
}