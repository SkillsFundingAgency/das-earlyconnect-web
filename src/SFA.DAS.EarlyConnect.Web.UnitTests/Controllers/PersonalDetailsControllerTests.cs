using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.Interfaces;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Extensions;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using static SFA.DAS.EarlyConnect.Web.ViewModels.IndustryViewModel;

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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Postcode = "12345", StudentSurvey= new StudentSurveyDto() };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.SchoolName(new SchoolNameViewModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<SchoolNameEditViewModel>());
            var viewModel = (SchoolNameEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.SchoolName, Is.EqualTo(queryResult.SchoolName));
        }

        [Test]
        public async Task SchoolName_Get_RedirectsToSurveyComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { StudentSurvey = new StudentSurveyDto { DateCompleted = DateTime.Now} };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.SchoolName(new SchoolNameViewModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task SchoolName_Post_RedirectsToCorrectRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();;
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Postcode = "12345", StudentSurvey = new StudentSurveyDto() };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.Postcode(new PostcodeViewModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<PostcodeEditViewModel>());
            var viewModel = (PostcodeEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.Postcode, Is.EqualTo(queryResult.Postcode));
        }

        [Test]
        public async Task Postcode_Get_RedirectsToSurveyComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { StudentSurvey = new StudentSurveyDto { DateCompleted = DateTime.Now } };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.Postcode(new PostcodeViewModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task Postcode_Post_RedirectsToCorrectRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);
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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);
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

            var result = await controller.Name(new TriageRouteModel { IsCheck = false, StudentSurveyId = new Guid()}) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Name_RedirectsTopSurveyComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto() { DateCompleted=DateTime.Now}
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            var result = await controller.Name(new TriageRouteModel { IsCheck = false, StudentSurveyId = new Guid() }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public void Authentication_NamePost_RedirectsToPostcodeRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Telephone = "07546666666" , StudentSurvey = new StudentSurveyDto()};
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.Telephone(new TriageRouteModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<TelephoneEditViewModel>());
            var viewModel = (TelephoneEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.Telephone, Is.EqualTo(queryResult.Telephone));
        }

        [Test]
        public async Task Telephone_Get_RedirectsToSurveyComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Telephone = "07546666666", StudentSurvey = new StudentSurveyDto { DateCompleted=DateTime.Now} };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.Telephone(new TriageRouteModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));

        }

        [Test]
        public async Task Telephone_Post_RedirectsToCheckYourAnswers()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);
            var viewModel = new TelephoneEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = true,
                IsOther = true
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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);
            var dateOfBirth = "05/12/1999".AsUKDate();

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { DateOfBirth = dateOfBirth, StudentSurvey = new StudentSurveyDto() };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.DateOfBirth(new TriageRouteModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<DateOfBirthEditViewModel>());
            var viewModel = (DateOfBirthEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.DateOfBirth.AsUKDate(), Is.EqualTo(queryResult.DateOfBirth));
        }

        [Test]
        public async Task DateOfBirth_Get_RedirectsToSurveyComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);
            var dateOfBirth = "05/12/1999".AsUKDate();

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { DateOfBirth = dateOfBirth, StudentSurvey=new StudentSurveyDto { DateCompleted =  DateTime.Now} };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.DateOfBirth(new TriageRouteModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task DateOfBirth_Post_RedirectsToCorrectRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

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
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);
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

        [Test]
        public async Task Industry_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 1,
                DateOfBirth = new DateTime(),
                FirstName = "First",
                LastName = "Last",
                Email = "test@example.com",
                Postcode = "CH67 6TY",
                Telephone = "123456",
                DataSource = "None",
                Industry = "Sector 1|Sector 8",
                StudentSurvey = new StudentSurveyDto()
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            AreasOfInterestModel areasofInterestModel = new AreasOfInterestModel()
            { 
                AreasOfInterest = new List<AreaOfInterest>()
                { 
                    new AreaOfInterest
                    {
                        Area = "Area 1",
                        Industry =  new List<Industry>
                        {
                            new Industry{ Title = "Industry A", Roles = "Role 1, Role 2, Role 3"},
                            new Industry { Title = "Industry B", Roles = "Role 4, Role 5, Role 6" },
                        }
                    },
                    new AreaOfInterest
                    {
                        Area = "Area 2",
                        Industry =  new List<Industry>
                        {
                            new Industry{ Title = "Industry C", Roles = "Role 7, Role 8, Role 9"},
                            new Industry { Title = "Industry D", Roles = "Role 10, Role 11, Role 12" },
                        }
                    }
                }
            };

            areasOfInterestHelper.Setup(a => a.LoadFromJSON(It.IsAny<string>())).Returns(areasofInterestModel);

            var result = await controller.Industry(new Guid(), false) as ViewResult;

            Assert.That(result, Is.Not.Null);

        }

        [Test]
        public async Task Industry_RedirectsToFormComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto() { DateCompleted=DateTime.Now}
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            var result = await controller.Industry(new Guid(), false) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public void IndustryPost_RedirectsToRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

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

            var model = new IndustryEditModel
            {
                IsCheck = false,
                StudentSurveyId = new Guid(),
                Sector = new List<string> { "Area 1", "Area 5", "Area 10" }
            };

            var result = controller.Industry(model).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.SchoolName_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public void IndustryPost_RedirectsToReview()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

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

            var model = new IndustryEditModel
            {
                IsCheck = true,
                StudentSurveyId = new Guid(),
                Sector = new List<string> { "Area 1", "Area 5", "Area 10" }
            };

            var result = controller.Industry(model).GetAwaiter().GetResult() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public void NamePost_RedirectsToCheckYourAnswersRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<PersonalDetailsController>>();
            var areasOfInterestHelper = new Mock<IJsonHelper>();

            var controller = new PersonalDetailsController(mediatorMock.Object, loggerMock.Object, areasOfInterestHelper.Object);

            var surveyResponse = new GetStudentTriageDataBySurveyIdResult {StudentSurvey = new StudentSurveyDto() };

            var createCommandResult = _fixture.Build<CreateStudentTriageDataCommandResult>()
                .With(x => x.Message, "Success")
                .Create();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(surveyResponse);

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);

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

    }
}