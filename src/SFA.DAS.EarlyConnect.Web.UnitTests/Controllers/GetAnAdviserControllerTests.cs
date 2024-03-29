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

        [Test]
        public void Index_ValidLepsCode_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectionServiceMock = new Mock<IDataProtectorService>();
            var authenticateServiecMock = new Mock<IAuthenticateService>();

            urlValidatorMock.Setup(x => x.IsValidLepsCode(It.IsAny<string>())).Returns(true);
            urlValidatorMock.Setup(x => x.IsValidLinkDate(It.IsAny<string>())).Returns(true);

            dataProtectionServiceMock.Setup(x => x.DecodeData(It.IsAny<string>())).Returns(new Guid().ToString() + "|" + DateTime.Now.ToString());

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectionServiceMock.Object, _configMock.Object, authenticateServiecMock.Object);

            var result = controller.Index("validLepsCode") as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Index_InvalidLepsCode_ReturnsNotFoundResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectionServiceMock = new Mock<IDataProtectorService>();
            var authenticateServiecMock = new Mock<IAuthenticateService>();

            urlValidatorMock.Setup(x => x.IsValidLepsCode(It.IsAny<string>())).Returns(false);

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectionServiceMock.Object, _configMock.Object, authenticateServiecMock.Object);
               
            var result = controller.Index("invalidLepsCode") as NotFoundResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetAnAdviser_Post_RedirectsToRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectionServiceMock = new Mock<IDataProtectorService>();
            var authenticateServiecMock = new Mock<IAuthenticateService>();

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectionServiceMock.Object, _configMock.Object, authenticateServiecMock.Object);

            var result = controller.GetAnAdviser_Post("someLepsCode") as RedirectToRouteResult;


            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Email_Get, Is.EqualTo(result.RouteName));
            Assert.That("someLepsCode", Is.EqualTo(result.RouteValues["lepsCode"]));
        }

        [Test]
        public async Task UcasIndex_ValidLink_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectionServiceMock = new Mock<IDataProtectorService>();
            var authenticateServiecMock = new Mock<IAuthenticateService>();

            urlValidatorMock.Setup(x => x.IsValidLepsCode(It.IsAny<string>())).Returns(true);
            urlValidatorMock.Setup(x => x.IsValidLinkDate(It.IsAny<string>())).Returns(true);

            var surveyId = new Guid();
            
            dataProtectionServiceMock.Setup(x => x.DecodeData(It.IsAny<string>())).Returns(surveyId + "|" + DateTime.Now.ToString());

            var queryResult = new GetStudentTriageDataBySurveyIdResult { StudentSurvey = new StudentSurveyDto() { DateCompleted = null} };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectionServiceMock.Object, _configMock.Object, authenticateServiecMock.Object);
            controller.ControllerContext = new ControllerContext() { HttpContext = mockContext.Object };

            var result = await controller.UCASIndex() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<TriageRouteModel>());
            var viewModel = (TriageRouteModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
        }

        [Test]
        public async Task UcasIndex_InvalidDate_ReturnsExpiredViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectionServiceMock = new Mock<IDataProtectorService>();
            var authenticateServiecMock = new Mock<IAuthenticateService>();

            urlValidatorMock.Setup(x => x.IsValidLepsCode(It.IsAny<string>())).Returns(true);
            urlValidatorMock.Setup(x => x.IsValidLinkDate(It.IsAny<string>())).Returns(false);

            dataProtectionServiceMock.Setup(x => x.DecodeData(It.IsAny<string>())).Returns(new Guid().ToString() + "|" + DateTime.Now.ToString());

            var queryResult = new GetStudentTriageDataBySurveyIdResult { };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectionServiceMock.Object, _configMock.Object, authenticateServiecMock.Object);
            controller.ControllerContext = new ControllerContext() { HttpContext = mockContext.Object };
 
            var result = await controller.UCASIndex() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Expired"));
        }

        [Test]
        public async Task UcasIndex_InvalidSurveyId_ReturnsLinkFaultViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectionServiceMock = new Mock<IDataProtectorService>();
            var authenticateServiecMock = new Mock<IAuthenticateService>();

            urlValidatorMock.Setup(x => x.IsValidLinkDate(It.IsAny<string>())).Returns(true);

            dataProtectionServiceMock.Setup(x => x.DecodeData(It.IsAny<string>())).Returns(new Guid().ToString() + "|" + DateTime.Now.ToString());

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectionServiceMock.Object, _configMock.Object, authenticateServiecMock.Object);
            controller.ControllerContext = new ControllerContext() { HttpContext = mockContext.Object };

            var result = await controller.UCASIndex() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("LinkFault"));
        }
    }

}