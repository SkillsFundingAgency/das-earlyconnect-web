using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnect.Web.ViewModels
{
    public class MoveEditViewModel : TriageRouteModel
    {
        public int? QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }
        public List<int> SelectedAnswerId { get; set; } = new List<int>();
        public List<AnswersDto> Answers { get; set; } = new List<AnswersDto>();
        public List<string> Relocate { get; set; } = new List<string>();
    }
}