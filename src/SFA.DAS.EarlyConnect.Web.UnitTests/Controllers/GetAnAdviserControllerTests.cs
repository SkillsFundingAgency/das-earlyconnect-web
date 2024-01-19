using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Infrastructure;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class GetAnAdviserControllerTests
    {
        [Test]
        public void Index_ValidLepsCode_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            urlValidatorMock.Setup(x => x.IsValidLepsCode(It.IsAny<string>())).Returns(true);

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object);

            var result = controller.Index("validLepsCode") as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Index_InvalidLepsCode_ReturnsNotFoundResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            urlValidatorMock.Setup(x => x.IsValidLepsCode(It.IsAny<string>())).Returns(false);

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object);

            var result = controller.Index("invalidLepsCode") as NotFoundResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetAnAdviser_Post_RedirectsToRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();

            var controller =
                new GetAnAdviserController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object);

            var result = controller.GetAnAdviser_Post("someLepsCode") as RedirectToRouteResult;


            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Dummy, Is.EqualTo(result.RouteName));
            Assert.That("someLepsCode", Is.EqualTo(result.RouteValues["lepsCode"]));
        }
    }

}