using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Controllers;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class GetAnAdviserControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<GetAnAdviserController>> _loggerMock;
        private Mock<IUrlValidator> _urlValidatorMock;
        private Mock<IAuthenticateService> _authenticateServiceMock;
        private GetAnAdviserController _controller;

        [SetUp]
        public void SetUp()
        {
            // Create all the mock dependencies
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<GetAnAdviserController>>();
            _urlValidatorMock = new Mock<IUrlValidator>();
            _authenticateServiceMock = new Mock<IAuthenticateService>();

            // Create the controller instance with all the mocked dependencies
            _controller = new GetAnAdviserController(
                _mediatorMock.Object,
                _loggerMock.Object,
                _urlValidatorMock.Object,
                _authenticateServiceMock.Object
            );
        }

        [Test]
        public void Index_WhenCalled_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void Index_WhenCalled_ReturnsIndexView()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Index_WhenCalled_DoesNotReturnNull()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Index_WhenCalled_ModelIsNull()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.Null);
        }

        [Test]
        public void Controller_CanBeCreated_WithAllDependencies()
        {
            // Arrange & Act
            var controller = new GetAnAdviserController(
                _mediatorMock.Object,
                _loggerMock.Object,
                _urlValidatorMock.Object,
                _authenticateServiceMock.Object
            );

            // Assert
            Assert.That(controller, Is.Not.Null);
            Assert.That(controller, Is.InstanceOf<GetAnAdviserController>());
        }
    }
}