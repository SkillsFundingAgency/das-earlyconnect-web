using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Extensions;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using System.Globalization;

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
        public async Task SchoolName_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Postcode = "12345" };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.SchoolName(new SchoolNameViewModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<SchoolNameEditViewModel>());
            var viewModel = (SchoolNameEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.SchoolName, Is.EqualTo(queryResult.SchoolName));
        }

        [Test]
        public async Task SchoolName_Post_RedirectsToCorrectRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SchoolNameEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = false,
            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = Survey
            });
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default)).ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.SchoolName(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.ApprenticeshipLevel_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
        }

        [Test]
        public async Task Postcode_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Postcode = "12345" };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.Postcode(new PostcodeViewModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<PostcodeEditViewModel>());
            var viewModel = (PostcodeEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.Postcode, Is.EqualTo(queryResult.Postcode));
        }

        [Test]
        public async Task Postcode_Post_RedirectsToCorrectRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new PostcodeEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = false,
            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey
                });
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default)).ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.Postcode(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Telephone_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
        }

        [Test]
        public async Task Postcode_Post_RedirectsToCheckYourAnswersRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new PostcodeEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = true,
            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey 
               });


            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default)).ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.Postcode(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
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

            var result = await controller.Name(new Guid(), false) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Authentication_NamePost_RedirectsToPostcodeRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

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
                StudentSurveyId = new Guid()
            };

            var result = controller.Name(model).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.DateOfBirth_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public void Authentication_NamePost_RedirectsToCheckYourAnswersRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult
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
                StudentSurveyId = new Guid()
            };

            var result = controller.Name(model).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
        }


        [Test]
        public async Task Telephone_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Telephone = "07546666666" };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.Telephone(new TriageRouteModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<TelephoneEditViewModel>());
            var viewModel = (TelephoneEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.Telephone, Is.EqualTo(queryResult.Telephone));
        }

        [Test]
        public async Task Telephone_Post_RedirectsToCheckYourAnswers()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new TelephoneEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = true,
            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey
                });
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default)).ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.Telephone(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
        }

        [Test]
        public async Task DateOfBirth_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);
            var dateOfBirth = "05/12/1999".AsDateTimeUk();

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { DateOfBirth = dateOfBirth };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.DateOfBirth(new TriageRouteModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<DateOfBirthEditViewModel>());
            var viewModel = (DateOfBirthEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.DateOfBirth.AsDateTimeUk(), Is.EqualTo(queryResult.DateOfBirth));
        }

        [Test]
        public async Task DateOfBirth_Post_RedirectsToCorrectRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new DateOfBirthEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = false,
                Day = "5",
                Month = "7",
                Year = "2004"
            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey
                });
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default)).ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.DateOfBirth(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Postcode_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
        }

        [Test]
        public async Task DateOfBirth_Post_RedirectsToCheckYourAnswersRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new DateOfBirthEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = true,
                Day = "5",
                Month = "7",
                Year = "2004"

            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey
                }); ;


            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default)).ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.DateOfBirth(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
        }
    }
}