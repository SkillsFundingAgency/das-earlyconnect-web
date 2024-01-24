using AutoFixture;
using Esfa.Recruit.Provider.Web.Configuration;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Configuration;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class AuthenticateControllerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void EmailAddress_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<AuthenticateController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectorServiceMock = new Mock<IDataProtectorService>();

            var controller =
                new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectorServiceMock.Object);

            var result = controller.EmailAddress("lepsCode") as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Authentication_EmailAddressPost_RedirectsToRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<AuthenticateController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectorServiceMock = new Mock<IDataProtectorService>();

            var createCommandResult = _fixture.Build<CreateOtherStudentTriageDataCommandResult>()
                .With(x => x.AuthCode, "1234")
                .With(x => x.StudentSurveyId, "1")
                .With(x => x.ExpiryDate, DateTime.Now)
                .Create();

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateOtherStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

            var controller =
                new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectorServiceMock.Object)
                {
                    TempData = tempData
                };

            var model = new EmailAddressViewModel
            {
                Email = "test@example.com",
                LepsCode = "someLepsCode"
            };

            var result = controller.EmailAddress(model).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Dummy, Is.EqualTo(result.RouteName));
            Assert.That(controller.TempData.Keys.First(), Is.EqualTo(TempDataKeys.TempDataAuthenticateModel));
        }
    }

}