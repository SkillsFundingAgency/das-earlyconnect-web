using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
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
        public void Authenticate_Get_ValidViewModel_ReturnsResult()
        {
            {
                var mediatorMock = new Mock<IMediator>();
                var loggerMock = new Mock<ILogger<AuthenticateController>>();
                var urlValidatorMock = new Mock<IUrlValidator>();
                var dataProtectorServiceMock = new Mock<IDataProtectorService>();
                var authenticateService = new Mock<IAuthenticateService>();

                var httpContext = new DefaultHttpContext();
                var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());


                var expectedViewModel = new AuthenticateViewModel();
                var serializedViewModel = JsonConvert.SerializeObject(expectedViewModel);

                tempData["AuthenticateModel"] = serializedViewModel;

                urlValidatorMock.Setup(uv => uv.IsValidLepsCode(It.IsAny<string>())).Returns(true);

                var controller =
                    new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object,
                        dataProtectorServiceMock.Object, authenticateService.Object)
                    {
                        TempData = tempData
                    };
                var result = controller.Authenticate() as ViewResult;

                Assert.That(result, Is.Not.Null);

            }
        }

        [Test]
        public async Task SendCode_ValidModel_ReturnsRedirectToRouteResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<AuthenticateController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectorServiceMock = new Mock<IDataProtectorService>();
            var authenticateService = new Mock<IAuthenticateService>();

            var createCommandResult = _fixture.Build<CreateOtherStudentTriageDataCommandResult>()
                .With(x => x.AuthCode, "1234")
                .With(x => x.StudentSurveyId, "1")
                .With(x => x.Expiry, DateTime.Now)
                .Create();

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateOtherStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

            var expectedViewModel = new AuthenticateViewModel();
            var serializedViewModel = JsonConvert.SerializeObject(expectedViewModel);

            tempData["AuthenticateModel"] = serializedViewModel;

            var controller =
                new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectorServiceMock.Object, authenticateService.Object)
                {
                    TempData = tempData
                };

            var model = new EmailAddressViewModel
            {
                Email = "test@example.com",
                LepsCode = "someLepsCode"
            };

            var result = await controller.SendCode() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Authenticate_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task Authenticate_Post_ValidAuthCode_ReturnsRedirectToRouteResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<AuthenticateController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectorServiceMock = new Mock<IDataProtectorService>();
            var authenticateService = new Mock<IAuthenticateService>();

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var viewModel = new AuthenticateViewModel
            {
                AuthCode = "decrypted_auth_code",
                Email = "ratheesh@education.com",
                ExpiryDate = DateTime.Now.AddDays(1),
                LepsCode = "E37000025",
                StudentSurveyId = "abc"
            };

            var serializedViewModel = JsonConvert.SerializeObject(viewModel);

            tempData["AuthenticateModel"] = serializedViewModel;

            var controller =
                new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object,
                    dataProtectorServiceMock.Object, authenticateService.Object)
                {
                    TempData = tempData
                };

            var authCodeViewModel = new AuthCodeViewModel
            {
                AuthCode = "decrypted_auth_code"
            };

            dataProtectorServiceMock.Setup(x => x.DecodeData(It.IsAny<string>())).Returns("decrypted_auth_code");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.Authenticate(authCodeViewModel) as RedirectToRouteResult;


            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.Name_Get));
        }

        [Test]
        public async Task Authenticate_Post_InvalidAuthCode_ReturnsRedirectToRouteResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<AuthenticateController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectorServiceMock = new Mock<IDataProtectorService>();
            var authenticateService = new Mock<IAuthenticateService>();

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var viewModel = new AuthenticateViewModel
            {
                AuthCode = "decrypted_auth_code1",
                Email = "ratheesh@education.com",
                ExpiryDate = DateTime.Now.AddDays(1),
                LepsCode = "E37000025",
                StudentSurveyId = "abc"
            };

            var serializedViewModel = JsonConvert.SerializeObject(viewModel);

            tempData["AuthenticateModel"] = serializedViewModel;

            var controller =
                new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object,
                    dataProtectorServiceMock.Object, authenticateService.Object)
                {
                    TempData = tempData
                };

            var authCodeViewModel = new AuthCodeViewModel
            {
                AuthCode = "decrypted_auth_code"
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.Authenticate(authCodeViewModel) as RedirectToRouteResult;


            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.Authenticate_Get));
        }

        [Test]
        public void EmailAddress_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<AuthenticateController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var dataProtectorServiceMock = new Mock<IDataProtectorService>();
            var authenticateService = new Mock<IAuthenticateService>();

            var controller =
                new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectorServiceMock.Object, authenticateService.Object);

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
            var authenticateService = new Mock<IAuthenticateService>();

            var createCommandResult = _fixture.Build<CreateOtherStudentTriageDataCommandResult>()
                .With(x => x.AuthCode, "1234")
                .With(x => x.StudentSurveyId, "1")
                .With(x => x.Expiry, DateTime.Now)
                .Create();

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateOtherStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

            var controller =
                new AuthenticateController(mediatorMock.Object, loggerMock.Object, urlValidatorMock.Object, dataProtectorServiceMock.Object, authenticateService.Object)
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
            Assert.That(RouteNames.Authenticate_Get, Is.EqualTo(result.RouteName));
            Assert.That(controller.TempData.Keys.First(), Is.EqualTo(TempDataKeys.TempDataAuthenticateModel));
        }
    }

}