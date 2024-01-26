using MediatR;
using SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateStudentTriageDataCommand : IRequest<Unit>
    {
        public StudentTriageData StudentData { get; set; }

        public Guid SurveyGuid { get; set; }
    }
}