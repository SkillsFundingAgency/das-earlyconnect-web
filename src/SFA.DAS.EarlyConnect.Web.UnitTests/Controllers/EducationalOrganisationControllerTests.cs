using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisations;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetEducationalOrganisations;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Controllers;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnectWeb.UnitTests.Controllers
{
    [TestFixture]
    public class EducationalOrganisationControllerTests
    {
        [Test]
        public async Task GetEducationalOrganisationsBySearch_ReturnsEducationalOrganisations_WhenCalledWithValidParameters()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var searchTerm = "Test School";
            var lepCode = "123";
            var educationalOrganisations = new List<EducationalOrganisationData>
        {
            new EducationalOrganisationData { Name = "Test School 1", AddressLine1 = "Address 1" },
            new EducationalOrganisationData { Name = "Test School 2", AddressLine1 = "Address 2" }
        };

            var educationalOrganisationsResponse = new GetEducationalOrganisationsResult
            {
                EducationalOrganisations = educationalOrganisations
            };

            mediatorMock
                .Setup(x => x.Send(It.Is<GetEducationalOrganisationsQuery>(q => q.SearchTerm == searchTerm && q.LepCode == lepCode), It.IsAny<CancellationToken>()))
                .ReturnsAsync(educationalOrganisationsResponse);

            var result = await controller.GetEducationalOrganisationsBySearch(searchTerm, lepCode) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(educationalOrganisations));

            mediatorMock.Verify(x => x.Send(It.Is<GetEducationalOrganisationsQuery>(q => q.SearchTerm == searchTerm && q.LepCode == lepCode), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetEducationalOrganisationsBySearch_ReturnsEmptyList_WhenNoOrganisationsFound()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var searchTerm = "Nonexistent School";
            var lepCode = "456";
            var educationalOrganisationsResponse = new GetEducationalOrganisationsResult
            {
                EducationalOrganisations = new List<EducationalOrganisationData>()
            };

            mediatorMock
                .Setup(x => x.Send(It.Is<GetEducationalOrganisationsQuery>(q => q.SearchTerm == searchTerm && q.LepCode == lepCode), It.IsAny<CancellationToken>()))
                .ReturnsAsync(educationalOrganisationsResponse);

            var result = await controller.GetEducationalOrganisationsBySearch(searchTerm, lepCode) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(new List<EducationalOrganisationData>()));

            mediatorMock.Verify(x => x.Send(It.Is<GetEducationalOrganisationsQuery>(q => q.SearchTerm == searchTerm && q.LepCode == lepCode), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SelectSchool_Get_RedirectsToFormCompleted_WhenSurveyIsCompleted()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var surveyId = Guid.NewGuid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto { DateCompleted = DateTime.Now }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(queryResult);

            var result = await controller.SelectSchool(new SelectSchoolViewModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.FormCompleted_Get));
        }

        [Test]
        public async Task SelectSchool_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var surveyId = Guid.NewGuid();

            var queryResult = new GetStudentTriageDataBySurveyIdResult
            {
                StudentSurvey = new StudentSurveyDto { DateCompleted = null },
                LepCode = "123",
                DataSource = Datasource.Others
            };

            var educationalOrganisationsResponse = new GetEducationalOrganisationsResult
            {
                EducationalOrganisations = new List<EducationalOrganisationData>
        {
            new EducationalOrganisationData { Name = "Test School", AddressLine1 = "123 Main St" }
        },
                TotalCount = 1
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(queryResult);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetEducationalOrganisationsQuery>(), default))
                .ReturnsAsync(educationalOrganisationsResponse);

            var result = await controller.SelectSchool(new SelectSchoolViewModel
            {
                StudentSurveyId = surveyId,
                SchoolSearchTerm = "Test",
                Page = 1,
                PageSize = 10,
                IsCheck = true
            }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<SelectSchoolEditViewModel>());

            var viewModel = (SelectSchoolEditViewModel)result.Model;

            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.SchoolSearchTerm, Is.EqualTo("Test"));
            Assert.That(viewModel.IsCheck, Is.True);
            Assert.That(viewModel.Page, Is.EqualTo(1));
            Assert.That(viewModel.PageSize, Is.EqualTo(10));
            Assert.That(viewModel.IsOther, Is.True);

            Assert.That(viewModel.PaginationViewModel, Is.Not.Null);
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToCheckYourAnswers_WhenJsEnabledAndIsCheckIsTrue()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Test School",
                IsJsEnabled = true,
                IsCheck = true
            };

            StudentSurveyDto Survey = new StudentSurveyDto();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                    .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                    {
                        StudentSurvey = Survey
                    });
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default))
                .ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.CheckYourAnswers_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToApprenticeshipLevel_WhenJsEnabledAndIsCheckIsFalse()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Test School",
                IsJsEnabled = true,
                IsCheck = false
            };

            StudentSurveyDto Survey = new StudentSurveyDto();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey
                });
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), default))
                .ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.ApprenticeshipLevel_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToCheckYourAnswers_WhenJsDisabledAndSchoolTermMatches()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Test School",
                IsJsEnabled = false,
                IsCheck = true
            };

            var studentSurveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                SchoolName = "Test School"
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(studentSurveyResponse);

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.CheckYourAnswers_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToSelectSchool_WhenNoMatchAndOrganisationsFound()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Different School",
                IsJsEnabled = false
            };

            var studentSurveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                SchoolName = "Test School",
                LepCode = "123"
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(studentSurveyResponse);

            var educationalOrganisationsResponse = new GetEducationalOrganisationsResult
            {
                TotalCount = 1,
                EducationalOrganisations = new List<EducationalOrganisationData>
        {
            new EducationalOrganisationData { Name = "Test School Organisation", AddressLine1 = "Test address" }
        }
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetEducationalOrganisationsQuery>(), default))
                .ReturnsAsync(educationalOrganisationsResponse);

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.SelectSchool_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
            Assert.That(result.RouteValues["SchoolSearchTerm"], Is.EqualTo(viewModel.SchoolSearchTerm));
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToNoResultsFound_WhenNoMatchAndNoOrganisationsFound()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();
            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Different School",
                IsJsEnabled = false
            };

            var studentSurveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                SchoolName = "Test School",
                LepCode = "123"
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default))
                .ReturnsAsync(studentSurveyResponse);

            var educationalOrganisationsResponse = new GetEducationalOrganisationsResult
            {
                TotalCount = 0,
                EducationalOrganisations = new List<EducationalOrganisationData>()
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetEducationalOrganisationsQuery>(), default))
                .ReturnsAsync(educationalOrganisationsResponse);

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.NoResultsFound_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
            Assert.That(result.RouteValues["SchoolSearchTerm"], Is.EqualTo(viewModel.SchoolSearchTerm));
        }

        [Test]
        public async Task SelectSchool_Post_RedirectsToCheckYourAnswers_WhenIsCheckIsTrue()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SelectSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SelectedSchool = "Test School,12345",
                IsCheck = true
            };

            StudentSurveyDto Survey = new StudentSurveyDto();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey
                });

            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.SelectSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.CheckYourAnswers_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
        }

        [Test]
        public async Task SelectSchool_Post_RedirectsToApprenticeshipLevel_WhenIsCheckIsFalse()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SelectSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SelectedSchool = "Test School,12345",
                IsCheck = false
            };

            StudentSurveyDto Survey = new StudentSurveyDto();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStudentTriageDataBySurveyIdResult
                {
                    StudentSurvey = Survey
                });

            mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentTriageDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateStudentTriageDataCommandResult());

            var result = await controller.SelectSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.ApprenticeshipLevel_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
        }

        [Test]
        public async Task SearchSchool_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { SchoolName = "Test School", StudentSurvey = new StudentSurveyDto() };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.SearchSchool(new SearchSchoolViewModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<SearchSchoolEditViewModel>());
            var viewModel = (SearchSchoolEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.SchoolSearchTerm, Is.EqualTo(queryResult.SchoolName));
        }

        [Test]
        public async Task SearchSchool_Get_RedirectsToSurveyComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { StudentSurvey = new StudentSurveyDto { DateCompleted = DateTime.Now } };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.SearchSchool(new SearchSchoolViewModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToCheckYourAnswers_WhenSchoolSearchTermMatchesAndIsCheckIsTrue()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Test School",
                IsCheck = true
            };

            var studentSurveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                SchoolName = "Test School"
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(studentSurveyResponse);

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.CheckYourAnswers_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToSelectSchool_WhenSchoolSearchTermDoesNotMatchAndOrganisationsAreFound()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Different School",
                IsCheck = false
            };

            var studentSurveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                SchoolName = "Test School",
                LepCode = "123"
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(studentSurveyResponse);

            var educationalOrganisationsResponse = new GetEducationalOrganisationsResult
            {
                TotalCount = 1,
                EducationalOrganisations = new List<EducationalOrganisationData>()
                {
                    new EducationalOrganisationData
                    {
                        Name = "Test School Organisation",
                        AddressLine1 = "Test address",
                    }
                }
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetEducationalOrganisationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(educationalOrganisationsResponse);

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.SelectSchool_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
            Assert.That(result.RouteValues["SchoolSearchTerm"], Is.EqualTo(viewModel.SchoolSearchTerm));
        }

        [Test]
        public async Task SearchSchool_Post_RedirectsToNoResultsFound_WhenSchoolSearchTermDoesNotMatchAndNoOrganisationsAreFound()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var viewModel = new SearchSchoolEditViewModel
            {
                StudentSurveyId = Guid.NewGuid(),
                SchoolSearchTerm = "Different School",
                IsCheck = false
            };

            var studentSurveyResponse = new GetStudentTriageDataBySurveyIdResult
            {
                SchoolName = "Test School",
                LepCode = "123"
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(studentSurveyResponse);

            var educationalOrganisationsResponse = new GetEducationalOrganisationsResult
            {
                EducationalOrganisations = new List<EducationalOrganisationData>()
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetEducationalOrganisationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(educationalOrganisationsResponse);

            var result = await controller.SearchSchool(viewModel) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(RouteNames.NoResultsFound_Get));
            Assert.That(result.RouteValues["StudentSurveyId"], Is.EqualTo(viewModel.StudentSurveyId));
            Assert.That(result.RouteValues["SchoolSearchTerm"], Is.EqualTo(viewModel.SchoolSearchTerm));
        }

        [Test]
        public async Task SchoolName_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { Postcode = "12345", StudentSurvey = new StudentSurveyDto() };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.SchoolName(new SchoolNameViewModel { StudentSurveyId = surveyId }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<SchoolNameEditViewModel>());
            var viewModel = (SchoolNameEditViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.SchoolName, Is.EqualTo(queryResult.SchoolName));
        }

        [Test]
        public void NoResultFound_Get_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var searchTerm = "Test";

            var result = controller.NoResultsFound(new NoResultsFoundViewModel { StudentSurveyId = surveyId, SchoolSearchTerm = searchTerm }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<NoResultsFoundViewModel>());
            var viewModel = (NoResultsFoundViewModel)result.Model;
            Assert.That(viewModel.StudentSurveyId, Is.EqualTo(surveyId));
            Assert.That(viewModel.SchoolSearchTerm, Is.EqualTo(searchTerm));
        }

        [Test]
        public async Task SchoolName_Get_RedirectsToSurveyComplete()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>();

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

            var surveyId = new Guid();
            var queryResult = new GetStudentTriageDataBySurveyIdResult { StudentSurvey = new StudentSurveyDto { DateCompleted = DateTime.Now } };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), default)).ReturnsAsync(queryResult);

            var result = await controller.SchoolName(new SchoolNameViewModel { StudentSurveyId = surveyId }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(RouteNames.FormCompleted_Get, Is.EqualTo(result.RouteName));
        }

        [Test]
        public async Task SchoolName_Post_RedirectsToCorrectRoute()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<EducationalOrganisationController>>(); ;

            var controller = new EducationalOrganisationController(mediatorMock.Object, loggerMock.Object);

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
    }
}