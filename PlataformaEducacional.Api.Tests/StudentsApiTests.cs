using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.Api.Tests.Config;
using System.Net;
using System.Net.Http.Json;

namespace PlataformaEducacional.Api.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class StudentsApiTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public StudentsApiTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Enroll student already enrolled in the course")]
        [Trait("Category", "API Integration - Student")]
        public async Task EnrollStudent_ShouldFail_WhenStudentAlreadyEnrolled()
        {
            // Arrange
            var courseId = await _testsFixture.GetCourseIdAsync();
            var data = new EnrollRequest
            {
                CourseId = courseId
            };

            await _testsFixture.StudentLoginAsync();
            _testsFixture.Client.AssignToken(_testsFixture.Token);

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync(
                "api/students/enroll", data);

            var errors = _testsFixture.GetErrors(
                await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Student already enrolled in the course!", errors);
        }

        [Fact(DisplayName = "Enroll student successfully")]
        [Trait("Category", "API Integration - Student")]
        public async Task EnrollStudent_ShouldSucceed()
        {
            // Arrange
            var courseId = await _testsFixture.GetCourseIdAsync();
            var data = new EnrollRequest
            {
                CourseId = courseId
            };

            await _testsFixture.RegisterNewStudentAsync();
            _testsFixture.Client.AssignToken(_testsFixture.Token);

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync(
                "api/students/enroll", data);

            var result =
                await _testsFixture.DeserializeResponse<ApiResponse<string>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.True(result.Success);
        }
    }
}
