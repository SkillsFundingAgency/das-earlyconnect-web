﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.RouteModel;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class SurveyControllerTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task ApprenticeshipLevel_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 1,
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
                        Id = 1,
                        SurveyId = 1001,
                        QuestionText = "Dummy Question 1",
                        GroupNumber = 1,
                        GroupLabel = "Group 1",
                        SummaryLabel = "Group 1 Summary",
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
                await controller.ApprenticeshipLevel(new ApprenticeshiplevelViewModel
                { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<ApprenticeshipLevelEditViewModel>());
            var viewModel = (ApprenticeshipLevelEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
        }

        [Test]
        public async Task ApprenticeshipLevel_Get_RedirectsToFormComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto
                {
                    SurveyId = 100,
                    DateCompleted = DateTime.Now,
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(createCommandResult);

            var result =
                await controller.ApprenticeshipLevel(new ApprenticeshiplevelViewModel
                { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task ApprenticeshipLevel_Post_RedirectsToCorrectRoute()
        {
            {
                var mediatorMock = new Mock<IMediator>();
                var loggerMock = new Mock<ILogger<SurveyController>>();

                var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

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

                var viewModel = new ApprenticeshipLevelEditViewModel
                {
                    StudentSurveyId = new Guid(),
                    IsCheck = false,
                    Question = new Questions
                    {
                        Id = 1,
                        SurveyId = 1,

                        ValidationMessage = "Message",
                        Answers = new List<Answers>
                        {
                            new Answers { Id = 1, QuestionId=2 ,IsSelected=true},
                         }
                    },
                };

                StudentSurveyDto Survey = new StudentSurveyDto();
                mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(createCommandResult);

                mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default))
                    .ReturnsAsync(new CreateStudentTriageDataCommandResult());

                var result = await controller.ApprenticeshipLevel(viewModel) as RedirectToRouteResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(RouteNames.AppliedFor_Get, Is.EqualTo(result.RouteName));
                Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
            }
        }

        [Test]
        public async Task AppliedFor_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 3,
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
                        QuestionTypeId = 1,
                        QuestionText = "Dummy Question 1",
                        ShortDescription = "",
                        SummaryLabel="Summary",
                        GroupLabel="Group Label",
                        GroupNumber= 1,
                        ValidationMessage = "None",
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
                await controller.AppliedFor(new AppliedForViewModel
                { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<AppliedForEditViewModel>());
            var viewModel = (AppliedForEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
        }
        [Test]
        public async Task AppliedFor_Get_RedirectsToFormComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto
                {
                    SurveyId = 100,
                    DateCompleted = DateTime.Now
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(createCommandResult);

            var result =
                await controller.AppliedFor(new AppliedForViewModel
                { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
           
        }

        [Test]
        public async Task AppliedFor_Post_RedirectsToCorrectRoute()
        {
            {
                var mediatorMock = new Mock<IMediator>();
                var loggerMock = new Mock<ILogger<SurveyController>>();

                var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

                var createCommandResult = new GetStudentTriageDataBySurveyIdResult
                {
                    Id = 3,
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
                        Id = 3,
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


                var viewModel = new AppliedForEditViewModel
                {
                    StudentSurveyId = new Guid(),
                    IsCheck = false,
                    Question = new Questions
                    {
                        Id = 1,
                        SurveyId = 1,

                        ValidationMessage = "Message",
                        Answers = new List<Answers>
                        {
                            new Answers { Id = 1, QuestionId=3 ,IsSelected=true},
                         }
                    },
                };

                StudentSurveyDto Survey = new StudentSurveyDto();
                mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(createCommandResult);

                mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default))
                    .ReturnsAsync(new CreateStudentTriageDataCommandResult());

                var result = await controller.AppliedFor(viewModel) as RedirectToRouteResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(RouteNames.Relocate_Get, Is.EqualTo(result.RouteName));
                Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
            }
        }

        [Test]
        public async Task Support_Get_RedirectsToFormComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto
                {
                    SurveyId = 100,
                    DateCompleted = DateTime.Now,
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(createCommandResult);

            var result =
                await controller.Support(new SupportViewModel
                { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task Spupport_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 4,
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
                        Id = 4,
                        SurveyId = 1001,
                        QuestionText = "Dummy Question 1",
                        GroupLabel = "Label A",
                        GroupNumber = 1,
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
                await controller.Support(new SupportViewModel
                { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<SupportEditViewModel>());
            var viewModel = (SupportEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
        }

        [Test]
        public async Task Support_Post_RedirectsToCorrectRoute()
        {
            {
                var mediatorMock = new Mock<IMediator>();
                var loggerMock = new Mock<ILogger<SurveyController>>();

                var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

                var createCommandResult = new GetStudentTriageDataBySurveyIdResult
                {
                    Id = 5,
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
                        Id = 5,
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

                var viewModel = new SupportEditViewModel
                {
                    StudentSurveyId = new Guid(),
                    IsCheck = false,
                    Question = new Questions
                    {
                        Id = 1,
                        SurveyId = 1,

                        ValidationMessage = "Message",
                        Answers = new List<Answers>
                        {
                            new Answers { Id = 1, QuestionId=3 ,IsSelected=true},
                         }
                    },
                };

                StudentSurveyDto Survey = new StudentSurveyDto();
                mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(createCommandResult);

                mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default))
                    .ReturnsAsync(new CreateStudentTriageDataCommandResult());

                var result = await controller.Support(viewModel) as RedirectToRouteResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
                Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
            }
        }


        [Test]
        public async Task Relocate_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 5,
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
                        Id = (int)SurveyPage.Page.Relocate,
                        SurveyId = 1001,
                        QuestionText = "Dummy Question Relocate",
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

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(createCommandResult);

            var result = await controller.Relocate(new TriageRouteModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<RelocateEditViewModel>());
            var viewModel = (RelocateEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));

        }

        [Test]
        public async Task Relocate_Get_RedirectsToFormComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto
                {
                    SurveyId = 100,
                    DateCompleted = DateTime.Now
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(createCommandResult);

            var result = await controller.Relocate(new TriageRouteModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task Relocate_Post_RedirectsToAreasOfSupport()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new RelocateEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = false,
                SelectedAnswerId = 2,
                Question = new Questions
                {
                    Id = 1,
                    SurveyId = 1,

                    ValidationMessage = "Message",
                    Answers = new List<Answers>
                        {
                            new Answers { Id = 1, QuestionId=(int)SurveyPage.Page.Relocate ,IsSelected=true},
                            new Answers { Id = 2, QuestionId=(int)SurveyPage.Page.Relocate ,IsSelected=false},
                         }
                },
            };

            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 5,
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
                        Id = (int)SurveyPage.Page.Relocate,
                        SurveyId = 1001,
                        QuestionText = "Dummy Question Relocate",
                        Answers = new List<AnswersDto>
                        {
                            new AnswersDto { Id = 1, QuestionId=(int)SurveyPage.Page.Relocate},
                         }
                    },
                },
                StudentSurvey = new StudentSurveyDto
                {
                    SurveyId = 100,
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(createCommandResult);

            var result = await controller.Relocate(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.Support_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
        }


        [Test]
        public async Task Relocate_Post_RedirectsToCheckYourAnswers()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var createCommandResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 5,
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
                        Id = (int)SurveyPage.Page.Relocate,
                        SurveyId = 1001,
                        QuestionText = "Dummy Question Relocate",
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

            var viewModel = new RelocateEditViewModel
            {
                StudentSurveyId = new Guid(),
                IsCheck = true,
                SelectedAnswerId = 2,
                Question = new Questions
                {
                    Id = 1,
                    SurveyId = 1,
                    ValidationMessage = "Message",
                    Answers = new List<Answers>
                        {
                            new Answers { Id = 1, QuestionId=3 ,IsSelected=false},
                            new Answers { Id = 2, QuestionId=3 ,IsSelected=false},
                         }
                },
            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCommandResult);
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default)).ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.Relocate(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.CheckYourAnswers_Get, Is.EqualTo(result.RouteName));
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(result.RouteValues["StudentSurveyId"]));
        }


        [Test]
        public async Task Confirmation_Get_ReturnsViewResult()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<SurveyController>>();

            var controller = new SurveyController(mediatorMock.Object, loggerMock.Object);

            var result =
                await controller.Confirmation() as ViewResult;

            Assert.That(result, Is.Not.Null);
           

        }
    }
}