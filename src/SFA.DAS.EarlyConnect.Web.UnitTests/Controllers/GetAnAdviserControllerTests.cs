using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Domain.Configuration;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class GetAnAdviserControllerTests
    {
        private Mock<IOptions<SFA.DAS.EarlyConnect.Domain.Configuration.EarlyConnectWeb>> _configMock;
        private Mock<HttpContext> mockContext;

        [SetUp]
        public void SetUp()
        {
            var config = new SFA.DAS.EarlyConnect.Domain.Configuration.EarlyConnectWeb
            {
                LepCodes = new LepsRegionCodes 
                {
                    GreaterLondon = "E37000051",
                    Lancashire = "E37000019",
                    NorthEast = "E37000025"
                }
            };
            _configMock = new Mock<IOptions<SFA.DAS.EarlyConnect.Domain.Configuration.EarlyConnectWeb>>();
            _configMock.Setup(ap => ap.Value).Returns(config);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(req => req.QueryString).Returns(new QueryString("?xyz"));

            mockContext = new Mock<HttpContext>();
            mockContext.Setup(con => con.Request).Returns(mockRequest.Object);

        }

    

  
 
    }

}