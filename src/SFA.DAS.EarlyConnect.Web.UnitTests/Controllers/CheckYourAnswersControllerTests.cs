using das_earlyconnect_web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class CheckYourAnswersControllerTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task CheckYourAnswers_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CheckYourAnswersController>>();

            var controller = new CheckYourAnswersController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 2,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Telephone = "123-456-7890",
                Email = "john.doe@example.com",
                Postcode = "12345",
                DataSource = "DummySource",
                Industry = "DummyIndustry",
                DateInterest = new DateTime(2022, 2, 1),
                SurveyQuestions = new List<SurveyQuestionsDto>
                {
                    new SurveyQuestionsDto
                    {
                        Id = 2,
                        SurveyId = 1001,
                        QuestionText = "Dummy Question 1",
                        Answers = new List<AnswersDto>
                        {
                            new AnswersDto { Id = 1, QuestionId=2 },
                         }
                    },
                },
                StudentSurvey = new StudentSurveyDto
                {
                    SurveyId = 100,
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(createCommandResult);

            var result =
                await controller.Check(surveyId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<CheckYourAnswersViewModel>());
            var viewModel = (CheckYourAnswersViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
        }

        [Test]
        public async Task CheckYourAnswers_Post_RedirectsToCorrectRoute()
        {
            {
                var mediatorMock = new Mock<IMediator>();
                var loggerMock = new Mock<ILogger<CheckYourAnswersController>>();

                var controller = new CheckYourAnswersController(mediatorMock.Object, loggerMock.Object);

                var createCommandResult = new GetStudentTriageDataBySurveyIdResult
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Telephone = "123-456-7890",
                    Email = "john.doe@example.com",
                    Postcode = "12345",
                    DataSource = "DummySource",
                    Industry = "DummyIndustry",
                    DateInterest = new DateTime(2022, 2, 1),
                    SurveyQuestions = new List<SurveyQuestionsDto>
                {
                    new SurveyQuestionsDto
                    {
                        Id = 2,
                        SurveyId = 1001,
                        QuestionText = "Dummy Question 1",
                        Answers = new List<AnswersDto>
                        {
                            new AnswersDto { Id = 1, QuestionId=2 },
                         }
                    },
                },
                    StudentSurvey = new StudentSurveyDto
                    {
                        SurveyId = 100,
                    }
                };

                var viewModel = new CheckYourAnswersViewModel
                {
                    StudentSurveyId = new Guid(),
                    IsCheck = false,
                };

                StudentSurveyDto Survey = new StudentSurveyDto();
                mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(createCommandResult);

                mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default))
                    .ReturnsAsync(new CreateStudentTriageDataCommandResult());

                var result = await controller.Check(viewModel) as RedirectToRouteResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(RouteNames.Confirmation_Get, Is.EqualTo(result.RouteName));
                Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
            }
        }
    }
}