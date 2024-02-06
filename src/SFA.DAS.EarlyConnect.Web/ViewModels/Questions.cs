using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class Questions
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public string? QuestionText { get; set; }
        public string? ShortDescription { get; set; }
        public string? SummaryLabel { get; set; }
        public string? ValidationMessage { get; set; }
        public int? DefaultToggleAnswerId { get; set; }
        public int SortOrder { get; set; }
        public List<Answers> Answers { get; set; } = new List<Answers>();

        public static implicit operator Questions(SurveyQuestionsDto source)
        {
            return new Questions
            {
                Id = source.Id,
                SurveyId = source.SurveyId,
                QuestionTypeId = source.QuestionTypeId,
                QuestionText = source.QuestionText,
                ShortDescription = source.ShortDescription,
                SummaryLabel = source.SummaryLabel,
                ValidationMessage = source.ValidationMessage,
                DefaultToggleAnswerId = source.DefaultToggleAnswerId,
                Answers = source.Answers.Select(c => (Answers)c).ToList()
            };
        }
    }
}
