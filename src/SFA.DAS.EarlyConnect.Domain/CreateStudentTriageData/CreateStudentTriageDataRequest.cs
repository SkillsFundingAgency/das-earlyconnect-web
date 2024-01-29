using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData
{
    public class CreateStudentTriageDataRequest : IPostApiRequest
    {
        public object Data { get; set; }
        public Guid SurveyGuid { get; set; }

        public CreateStudentTriageDataRequest(Guid surveyGuid, StudentTriageData studentData)
        {
            SurveyGuid = surveyGuid;
            Data = studentData;
        }

        public string PostUrl => $"student-triage-data/{SurveyGuid}";
    }
}