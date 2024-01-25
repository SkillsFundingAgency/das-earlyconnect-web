using MediatR;
using SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.Extensions;
using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateOtherStudentTriageDataCommandHandler : IRequestHandler<CreateOtherStudentTriageDataCommand, CreateOtherStudentTriageDataCommandResult>
    {

        private readonly IApiClient _apiClient;

        public CreateOtherStudentTriageDataCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateOtherStudentTriageDataCommandResult> Handle(CreateOtherStudentTriageDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Post<CreateOtherStudentTriageDataResponse>(new CreateOtherStudentTriageDataRequest(request.StudentTriageData), true);

            response.EnsureSuccessStatusCode();

            return new CreateOtherStudentTriageDataCommandResult
            {
                StudentSurveyId = response.Body.StudentSurveyId,
                AuthCode = response.Body.AuthCode,
                Expiry = response.Body.Expiry
            };
        }
    }
}
