using MediatR;
using SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateStudentTriageDataCommand : IRequest<CreateStudentTriageDataCommandResult>
    {
        public StudentTriageData StudentData { get; set; }

        public Guid SurveyGuid { get; set; }
    }
}