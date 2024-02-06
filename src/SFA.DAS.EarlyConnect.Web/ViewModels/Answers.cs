using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class Answers
    {
        public int Serial { get; set; }
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public string? ShortDescription { get; set; }
        public int? GroupNumber { get; set; }
        public string? GroupLabel { get; set; }
        public bool IsSelected { get; set; }
        public int StudentAnswerId { get; set; }
        public AnswerGroup.Group ToggleAnswer { get; set; }
        public int SortOrder { get; set; }

        public static implicit operator Answers(AnswersDto source)
        {
            return new Answers
            {
                Id = source.Id,
                QuestionId = source.QuestionId,
                AnswerText = source.AnswerText,
                ShortDescription = source.ShortDescription,
                GroupNumber = source.GroupNumber,
                GroupLabel = source.GroupLabel,
                SortOrder = source.SortOrder,
            };
        }
    }
}
