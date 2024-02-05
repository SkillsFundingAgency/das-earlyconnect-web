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
            IsOther ? RouteNames.School_Get :
            RouteNames.School_Get;

        public static implicit operator ApprenticeshipLevelEditViewModel(GetStudentTriageDataBySurveyIdResult request)
        {
            var apprenticeshiplevelEditViewModel = new ApprenticeshipLevelEditViewModel
            {
                Question = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Apprenticeshiplevel)
            };

            var surveyQuestion = request.SurveyQuestions.FirstOrDefault(q => q.Id == (int)SurveyPage.Page.Apprenticeshiplevel);

            if (surveyQuestion != null)
            {
                var existingAnswers = apprenticeshiplevelEditViewModel.Question.Answers
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

                apprenticeshiplevelEditViewModel.Question.Answers = existingAnswers;
            }

            return apprenticeshiplevelEditViewModel;
        }
    }
}