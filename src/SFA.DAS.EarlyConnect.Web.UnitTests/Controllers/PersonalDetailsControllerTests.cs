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
    public class PersonalDetailsControllerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Name_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller =
                new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 1,
                DateOfBirth = new DateTime(),
                Email = "",
                Postcode = "",
                Telephone = "",
                DataSource = "",
                Industry = "",
                StudentSurvey = new StudentSurveyDto()
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            var result = await controller.Name("12345", false) as ViewResult;

            Assert.That(result, Is.Not.Null);

            
        }

        [Test]
        public void Authentication_NamePost_RedirectsToPostcodeRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult 
            {   Id = 1,
                DateOfBirth = new DateTime(),
                Email = "",
                Postcode = "",
                Telephone = "",
                DataSource = "",
                Industry = "",
                StudentSurvey = new StudentSurveyDto()
            };

            var createCommandResult = _fixture.Build<CreateStudentTriageDataCommandResult>()
                .With(x => x.Message, "Success")
                .Create();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var model = new NameViewModel
            {
                FirstName = "Mr",
                LastName = "Greene",
                IsCheck = false,
                StudentSurveyId = new Guid().ToString()
            };

            var result = controller.Name(model).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.StartAgain_Get, Is.EqualTo(result.RouteName));
        }

        public void Authentication_NamePost_RedirectsToCheckYourAnswersRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult();

            var createCommandResult = _fixture.Build<CreateStudentTriageDataCommandResult>()
                .With(x => x.Message, "Success")
                .Create();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var model = new NameViewModel
            {
                FirstName = "Mr",
                LastName = "Greene",
                IsCheck = true,
                StudentSurveyId = new Guid().ToString()
            };

            var result = controller.Name(model).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
        }
    }
}