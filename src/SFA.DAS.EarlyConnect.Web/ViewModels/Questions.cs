﻿using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;

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
        public string? GroupLabel { get; set; }
        public int GroupNumber { get; set; }
        public string? ValidationMessage { get; set; }
        public int? DefaultToggleAnswerId { get; set; }
        public int SortOrder { get; set; }
        public SurveyQuestionType.Type? QuestionType { get; set; }
        public string? RouteName { get; set; }
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
                GroupLabel = source.GroupLabel,
                GroupNumber = source.GroupNumber,
                ValidationMessage = source.ValidationMessage,
                DefaultToggleAnswerId = source.DefaultToggleAnswerId,
                RouteName = GetRouteName(source.Id),
                QuestionType = GetQuestionType(source.QuestionTypeId),
                Answers = source.Answers.Select(c => (Answers)c).ToList()
            };
        }
        private static string GetRouteName(int surveyQuestionId)
        {
            return surveyQuestionId switch
            {
                (int)SurveyPage.Page.Apprenticeshiplevel => RouteNames.ApprenticeshipLevel_Get,
                (int)SurveyPage.Page.AppliedFor => RouteNames.AppliedFor_Get,
                (int)SurveyPage.Page.Relocate => RouteNames.Relocate_Get,
                (int)SurveyPage.Page.Support => RouteNames.Support_Get,
                _ => string.Empty
            };
        }
        private static SurveyQuestionType.Type GetQuestionType(int questionTypeId)
        {
            return questionTypeId switch
            {
                (int)SurveyQuestionType.Type.Checkbox => SurveyQuestionType.Type.Checkbox,
                (int)SurveyQuestionType.Type.Radio => SurveyQuestionType.Type.Radio,
                _ => SurveyQuestionType.Type.Default
            };
        }

    }
}
