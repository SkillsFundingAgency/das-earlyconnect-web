using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.ApiResponse;
using SFA.DAS.EarlyConnect.Domain.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.Exceptions;
using SFA.DAS.EarlyConnect.Domain.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.UnitTests.Commands
{
    public class CreateOtherStudentTriageDataCommandHandlerTest
    {
        private Fixture _fixture;
        public Mock<ILogger<CreateOtherStudentTriageDataCommandHandler>> _logger;
        private Mock<IApiClient> _api;
        private CreateOtherStudentTriageDataCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _logger = new Mock<ILogger<CreateOtherStudentTriageDataCommandHandler>>();
            _api = new Mock<IApiClient>();
            _handler = new CreateOtherStudentTriageDataCommandHandler(_api.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        }


        [Test]
        public async Task Handle_GivenValidRequest_ReturnsExpectedResult()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var command = new CreateOtherStudentTriageDataCommand();

            var response = new CreateOtherStudentTriageDataResponse
            {
                AuthCode = "1234",
                Expiry = DateTime.UtcNow,
                StudentSurveyId = "1"
            };
 
            var expectedResponse = new ApiResponse<CreateOtherStudentTriageDataResponse>(response, System.Net.HttpStatusCode.Created, string.Empty);

            var handler = new CreateOtherStudentTriageDataCommandHandler(mockApiClient.Object);

            mockApiClient
                .Setup(api => api.Post<CreateOtherStudentTriageDataResponse>(It.IsAny<CreateOtherStudentTriageDataRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(expectedResponse.Body.StudentSurveyId, result.StudentSurveyId);
            Assert.AreEqual(expectedResponse.Body.AuthCode, result.AuthCode);
            Assert.AreEqual(expectedResponse.Body.Expiry, result.Expiry);
        }

        [Test]
        public async Task Handle_GivenBadRequest_ThrowsException()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var command = new CreateOtherStudentTriageDataCommand();
            var response = new CreateOtherStudentTriageDataResponse
            {
                AuthCode = "1234",
                Expiry = DateTime.UtcNow,
                StudentSurveyId = "1"
            };

            var expectedResponse = new ApiResponse<CreateOtherStudentTriageDataResponse>(response, System.Net.HttpStatusCode.BadRequest, string.Empty);
            var handler = new CreateOtherStudentTriageDataCommandHandler(mockApiClient.Object);

            mockApiClient
                .Setup(api => api.Post<CreateOtherStudentTriageDataResponse>(It.IsAny<CreateOtherStudentTriageDataRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedResponse);

            // Act & Assert
            Assert.ThrowsAsync<ApiResponseException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}
