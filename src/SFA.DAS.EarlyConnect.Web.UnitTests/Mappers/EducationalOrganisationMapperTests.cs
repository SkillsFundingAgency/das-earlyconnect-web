using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Domain.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.EarlyConnect.Web.Mappers;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.UnitTests.Mappers
{
    [TestFixture]
    public class EducationalOrganisationControllerTests
    {
        [Test]
        public void MapFromSelectSchoolNameRequest_ShouldMapCorrectly()
        {
            var studentSurveyId = Guid.NewGuid();
            var viewModel = new SelectSchoolEditViewModel
            {
                SelectedSchool = "Test School",
                StudentSurveyId = studentSurveyId
            };

            var studentTriageDataResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                SchoolName = "Test School",
                Email = "test@example.com",
                Postcode = "AB12 3CD",
                Telephone = "0123456789",
                DataSource = Datasource.Others,
                Industry = "Education",
                StudentSurvey = new StudentSurveyDto(),
            };

            var result = viewModel.MapFromSelectSchoolNameRequest(studentTriageDataResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(studentTriageDataResult.Id));
            Assert.That(result.FirstName, Is.EqualTo(studentTriageDataResult.FirstName));
            Assert.That(result.LastName, Is.EqualTo(studentTriageDataResult.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(studentTriageDataResult.DateOfBirth));
            Assert.That(result.SchoolName, Is.EqualTo(viewModel.SelectedSchool));
            Assert.That(result.Email, Is.EqualTo(studentTriageDataResult.Email));
            Assert.That(result.Postcode, Is.EqualTo(studentTriageDataResult.Postcode));
            Assert.That(result.Telephone, Is.EqualTo(studentTriageDataResult.Telephone));
            Assert.That(result.DataSource, Is.EqualTo(studentTriageDataResult.DataSource));
            Assert.That(result.Industry, Is.EqualTo(studentTriageDataResult.Industry));
            Assert.That(result.StudentSurvey, Is.EqualTo(studentTriageDataResult.StudentSurvey));
            Assert.That(result.StudentSurvey.ResponseAnswers, Is.Not.Null);
        }
        [Test]
        public void MapFromSearchSchoolNameRequest_ShouldMapCorrectly()
        {
            var studentSurveyId = Guid.NewGuid();
            var viewModel = new SearchSchoolEditViewModel
            {
                SchoolSearchTerm = "Test School",
                SelectedUrn = "Test",
                StudentSurveyId = studentSurveyId
            };

            var studentTriageDataResult = new GetStudentTriageDataBySurveyIdResult
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                SchoolName = "Test School",
                URN = "Test",
                Email = "test@example.com",
                Postcode = "AB12 3CD",
                Telephone = "0123456789",
                DataSource = Datasource.Others,
                Industry = "Education",
                StudentSurvey = new StudentSurveyDto(),
            };

            var result = viewModel.MapFromSearchSchoolNameRequest(studentTriageDataResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(studentTriageDataResult.Id));
            Assert.That(result.FirstName, Is.EqualTo(studentTriageDataResult.FirstName));
            Assert.That(result.LastName, Is.EqualTo(studentTriageDataResult.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(studentTriageDataResult.DateOfBirth));
            Assert.That(result.SchoolName, Is.EqualTo(viewModel.SchoolSearchTerm));
            Assert.That(result.URN, Is.EqualTo(viewModel.SelectedUrn));
            Assert.That(result.Email, Is.EqualTo(studentTriageDataResult.Email));
            Assert.That(result.Postcode, Is.EqualTo(studentTriageDataResult.Postcode));
            Assert.That(result.Telephone, Is.EqualTo(studentTriageDataResult.Telephone));
            Assert.That(result.DataSource, Is.EqualTo(studentTriageDataResult.DataSource));
            Assert.That(result.Industry, Is.EqualTo(studentTriageDataResult.Industry));
            Assert.That(result.StudentSurvey, Is.EqualTo(studentTriageDataResult.StudentSurvey));
            Assert.That(result.StudentSurvey.ResponseAnswers, Is.Not.Null);
        }
    }
}
