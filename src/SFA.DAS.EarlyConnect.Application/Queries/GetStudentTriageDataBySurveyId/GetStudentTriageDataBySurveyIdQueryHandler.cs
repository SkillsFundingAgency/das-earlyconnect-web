using MediatR;
using SFA.DAS.EarlyConnect.Domain.Extensions;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQueryHandler : IRequestHandler<GetStudentTriageDataBySurveyIdQuery, GetStudentTriageDataBySurveyIdResult>
    {
        private readonly IApiClient _apiClient;

        public GetStudentTriageDataBySurveyIdQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetStudentTriageDataBySurveyIdResult> Handle(GetStudentTriageDataBySurveyIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetStudentTriageDataBySurveyIdResponse>(new GetStudentTriageDataBySurveyIdRequest(request.SurveyGuid));

            result.EnsureSuccessStatusCode();

            return new GetStudentTriageDataBySurveyIdResult
            {
                Id = result.Body.Id,
                LepsId = result.Body.LepsId,
                LogId = result.Body.LogId,
                FirstName = result.Body.FirstName,
                LastName = result.Body.LastName,
                DateOfBirth = result.Body.DateOfBirth,
                Email = result.Body.Email,
                Telephone = result.Body.Telephone,
                Postcode = result.Body.Postcode,
                DataSource = result.Body.DataSource,
                Industry = result.Body.Industry,
                DateInterest = result.Body.DateInterest,
                SchoolName  = result.Body.SchoolName,
                StudentSurvey = result.Body.StudentSurvey,
                SurveyQuestions = result.Body.SurveyQuestions
            };
        }
    }
}