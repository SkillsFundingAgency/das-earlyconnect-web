using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData
{
    public class CreateOtherStudentTriageDataRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateOtherStudentTriageDataRequest(OtherStudentTriageData otherStudentTriageData)
        {
            Data = otherStudentTriageData;
        }

        public string PostUrl => "student-triage-data/survey-create";
    }
}