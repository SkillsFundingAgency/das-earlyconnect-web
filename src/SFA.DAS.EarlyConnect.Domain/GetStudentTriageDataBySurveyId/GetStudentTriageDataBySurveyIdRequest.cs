using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdRequest : IGetApiRequest
    {
        public Guid SurveyGuid { get; set; }
        public GetStudentTriageDataBySurveyIdRequest(Guid surveyGuid)
        {
            SurveyGuid = surveyGuid;
        }
        public string GetUrl => $"student-triage-data/{SurveyGuid}";
    }
}