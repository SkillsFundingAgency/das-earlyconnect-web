using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;

namespace SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData
{
    public class StudentTriageData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string DataSource { get; set; }
        public string SchoolName { get; set; }
        public string Industry { get; set; }
        public StudentSurveyDto StudentSurvey { get; set; }
    }
}