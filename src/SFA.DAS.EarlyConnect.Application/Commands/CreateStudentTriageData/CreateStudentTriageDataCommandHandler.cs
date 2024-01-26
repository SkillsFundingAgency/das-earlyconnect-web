using MediatR;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.Extensions;
using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateStudentTriageDataCommandHandler : IRequestHandler<CreateStudentTriageDataCommand, Unit>
    {

        private readonly IApiClient _apiClient;

        public CreateStudentTriageDataCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(CreateStudentTriageDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Post<CreateStudentDataResponse>(new CreateStudentTriageDataRequest(request.SurveyGuid, request.StudentData), true);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
