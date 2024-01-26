using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.ApiResponse;
using SFA.DAS.EarlyConnect.Domain.CreateStudentTriageData;
using SFA.DAS.EarlyConnect.Domain.Exceptions;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using System.Net;
using System.Reflection.Metadata;

namespace SFA.DAS.EarlyConnect.Application.UnitTests.Commands
{
    public class CreateStudentTriageDataCommandHandlerTest
    {
        private Fixture _fixture;
        public Mock<ILogger<CreateStudentTriageDataCommandHandler>> _logger;
        private Mock<IApiClient> _api;
        private CreateStudentTriageDataCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _logger = new Mock<ILogger<CreateStudentTriageDataCommandHandler>>();
            _api = new Mock<IApiClient>();
            _handler = new CreateStudentTriageDataCommandHandler(_api.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }


        [Test]
        public async Task Handle_GivenValidRequest_ReturnsExpectedResult()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var command = new CreateStudentTriageDataCommand();
            var handler = new CreateStudentTriageDataCommandHandler(mockApiClient.Object);
            var expectedResponse = new CreateStudentTriageDataResponse
            {
                Message = "Success"
            };

            mockApiClient
                .Setup(api => api.Post<CreateStudentTriageDataResponse>(It.IsAny<CreateStudentTriageDataRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(new ApiResponse<CreateStudentTriageDataResponse>(expectedResponse, HttpStatusCode.Created, string.Empty));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            mockApiClient.Verify(x => x.Post<CreateStudentTriageDataResponse>(It.IsAny<CreateStudentTriageDataRequest>(), It.IsAny<bool>()), Times.Once);
            Assert.AreEqual(expectedResponse.Message, result.Message);
        }

        [Test]
        public async Task Handle_GivenBadRequest_ThrowsException()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var command = new CreateStudentTriageDataCommand();
            var expectedResponse = new ApiResponse<CreateStudentTriageDataResponse>(new CreateStudentTriageDataResponse(), System.Net.HttpStatusCode.BadRequest, string.Empty);

            var handler = new CreateStudentTriageDataCommandHandler(mockApiClient.Object);

            mockApiClient
                .Setup(api => api.Post<CreateStudentTriageDataResponse>(It.IsAny<CreateStudentTriageDataRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedResponse);

            // Act & Assert
            Assert.ThrowsAsync<ApiResponseException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}
