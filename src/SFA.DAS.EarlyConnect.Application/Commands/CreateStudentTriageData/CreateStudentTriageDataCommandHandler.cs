using MediatR;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.Extensions;
using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateStudentTriageDataCommandHandler : IRequestHandler<CreateStudentTriageDataCommand, CreateStudentTriageDataCommandResult>
    {

        private readonly IApiClient _apiClient;

        public CreateStudentTriageDataCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateStudentTriageDataCommandResult> Handle(CreateStudentTriageDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Post<CreateStudentTriageDataResponse>(new CreateStudentTriageDataRequest(request.SurveyGuid, request.StudentData), true);

            response.EnsureSuccessStatusCode();

            return new CreateStudentTriageDataCommandResult
            {
                Message = response.Body.Message
            };
        }
    }
}
