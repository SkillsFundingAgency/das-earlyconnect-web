using System.Net;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations;
using SFA.DAS.EarlyConnect.Domain.ApiResponse;
using SFA.DAS.EarlyConnect.Domain.GetEducationalOrganisations;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Domain.Exceptions;

namespace SFA.DAS.EarlyConnect.Application.UnitTests.Queries
{
    [TestFixture]
    public class GetEducationalOrganisationsQueryHandlerTests
    {
        private Mock<IApiClient> _apiClientMock;
        private GetEducationalOrganisationsQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _apiClientMock = new Mock<IApiClient>();
            _handler = new GetEducationalOrganisationsQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ReturnsExpectedResult()
        {
            var query = new GetEducationalOrganisationsQuery
            {
                LepCode = "LEP123",
                SearchTerm = "Test School",
                Page = 1,
                PageSize = 10
            };

            var educationalOrganisationsResponse = new GetEducationalOrganisationsResponse
            {
                EducationalOrganisations = new List<EducationalOrganisationData>
                {
                    new EducationalOrganisationData
                    {
                        Name = "Test School Organisation",
                        AddressLine1 = "Test address"
                    }
                }
            };


            _apiClientMock
                .Setup(x => x.Get<GetEducationalOrganisationsResponse>(It.IsAny<GetEducationalOrganisationsRequest>()))
                .ReturnsAsync(new ApiResponse<GetEducationalOrganisationsResponse>(educationalOrganisationsResponse, HttpStatusCode.OK, string.Empty));

            var result = await _handler.Handle(query, CancellationToken.None);

            _apiClientMock.Verify(x => x.Get<GetEducationalOrganisationsResponse>(It.IsAny<GetEducationalOrganisationsRequest>()), Times.Once);

            Assert.That(result.EducationalOrganisations.Count, Is.EqualTo(1));
            var organisation = result.EducationalOrganisations.ElementAt(0); // Use ElementAt to access the first item
            Assert.That(organisation.Name, Is.EqualTo("Test School Organisation"));
            Assert.That(organisation.AddressLine1, Is.EqualTo("Test address"));
        }


        [Test]
        public void Handle_GivenBadRequest_ThrowsApiResponseException()
        {
            var query = new GetEducationalOrganisationsQuery
            {
                LepCode = "LEP123",
                SearchTerm = "Test School",
                Page = 1,
                PageSize = 10
            };

            var badResponse = new ApiResponse<GetEducationalOrganisationsResponse>(null, HttpStatusCode.BadRequest, "Bad Request");

            _apiClientMock
                .Setup(x => x.Get<GetEducationalOrganisationsResponse>(It.IsAny<GetEducationalOrganisationsRequest>()))
                .ReturnsAsync(badResponse);

            Assert.ThrowsAsync<ApiResponseException>(async () => await _handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public async Task Handle_GivenEmptyResponse_ReturnsEmptyList()
        {
            var query = new GetEducationalOrganisationsQuery
            {
                LepCode = "LEP123",
                SearchTerm = "NonExistent School",
                Page = 1,
                PageSize = 10
            };

            var emptyResponse = new GetEducationalOrganisationsResponse();
            emptyResponse.EducationalOrganisations = new List<EducationalOrganisationData>();

            _apiClientMock
                .Setup(x => x.Get<GetEducationalOrganisationsResponse>(It.IsAny<GetEducationalOrganisationsRequest>()))
                .ReturnsAsync(new ApiResponse<GetEducationalOrganisationsResponse>(emptyResponse, HttpStatusCode.OK, string.Empty));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result.EducationalOrganisations, Is.Empty);
            _apiClientMock.Verify(x => x.Get<GetEducationalOrganisationsResponse>(It.IsAny<GetEducationalOrganisationsRequest>()), Times.Once);
        }
    }
}
