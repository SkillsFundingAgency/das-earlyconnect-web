using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdRequest : IGetApiRequest
    {
        public string SurveyGuid { get; set; }
        public GetStudentTriageDataBySurveyIdRequest(string surveyGuid)
        {
            SurveyGuid = surveyGuid;
        }
        public string GetUrl => $"student-triage-data/{SurveyGuid}";
    }
}