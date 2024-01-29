using AutoFixture;
using Esfa.Recruit.Provider.Web.Configuration;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Application.Services;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Configuration;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class TriageDataControllerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Industry_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<TriageDataController>>();

            var controller =
                new TriageDataController(mediatorMock.Object, loggerMock.Object);

            var result = await controller.Industry("12345", false) as ViewResult;

            Assert.That(result, Is.Not.Null);

        }

        [Test]
        public void IndustryPost_RedirectsToRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<TriageDataController>>();


            var surveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 1,
                DateOfBirth = new DateTime(),
                Email = "",
                Postcode = "",
                Telephone = "",
                DataSource = "",
                Industry = "Area 1|Area 2",
                StudentSurvey = new StudentSurveyDto()
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            var createCommandResult = _fixture.Build<CreateStudentTriageDataCommandResult>()
               .With(x => x.Message, "Success")
               .Create();

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

            var controller =
                new TriageDataController(mediatorMock.Object, loggerMock.Object);

            var model = new IndustryViewModel
            {
                IsCheck = false,
                StudentSurveyId = new Guid().ToString()
            };

            var result = controller.Industry(model, new List<string> { "Area 1", "Area 5", "Area 10"}).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Dummy, Is.EqualTo(result.RouteName));
        }

        [Test]
        public void IndustryPost_RedirectsToReview()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<TriageDataController>>();


            var surveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 1,
                DateOfBirth = new DateTime(),
                Email = "",
                Postcode = "",
                Telephone = "",
                DataSource = "",
                Industry = "Area 1|Area 2",
                StudentSurvey = new StudentSurveyDto()
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            var createCommandResult = _fixture.Build<CreateStudentTriageDataCommandResult>()
               .With(x => x.Message, "Success")
               .Create();

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

            var controller =
                new TriageDataController(mediatorMock.Object, loggerMock.Object);

            var model = new IndustryViewModel
            {
                IsCheck = true,
                StudentSurveyId = new Guid().ToString()
            };

            var result = controller.Industry(model, new List<string> { "Area 1", "Area 5", "Area 10" }).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
        }
    }

}