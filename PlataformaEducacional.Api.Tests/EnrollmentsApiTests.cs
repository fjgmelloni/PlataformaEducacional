using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.Api.Tests.Config;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;
using PlataformaEducacional.StudentAdministration.Domain;
using System.Net;
using System.Net.Http.Json;

namespace PlataformaEducacional.Api.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class EnrollmentsApiTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public EnrollmentsApiTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Process payment should fail when student is not enrolled")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task ProcessPayment_ShouldFail_WhenStudentIsNotEnrolled()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();

            var request = new ProcessPaymentRequest
            {
                CardName = "Test",
                CardNumber = "4111111111111111",
                CardExpiration = "12/30",
                CardCvv = "123",
                Total = 500
            };

            await _testsFixture.RegisterNewStudentAsync();
            _testsFixture.Client.AssignToken(_testsFixture.Token);

            // Act
            var response = await _testsFixture.Client
                .PostAsJsonAsync($"api/enrollments/{enrollmentId}/payment", request);

            var errors = _testsFixture.GetErrors(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Enrollment not found.", errors);
        }

        [Fact(DisplayName = "Process payment")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task ProcessPayment()
        {
            // Arrange
            var enrollmentId = await _testsFixture.PendingEnrollmentIdAsync();

            var request = new ProcessPaymentRequest
            {
                CardName = "Test",
                CardNumber = "4111111111111111",
                CardExpiration = "12/30",
                CardCvv = "123",
                Total = 500
            };

            // Act
            var response = await _testsFixture.Client
                .PostAsJsonAsync($"api/enrollments/{enrollmentId}/payment", request);

            var errors = _testsFixture.GetErrors(await response.Content.ReadAsStringAsync());

            // Assert
            if (errors.Any())
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Contains("Payment was declined", errors);
            }
            else
            {
                var result =
                    await _testsFixture.DeserializeResponse<ApiResponse<string>>(response);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(result.Success);
            }
        }

        [Fact(DisplayName = "Perform lesson successfully")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task PerformLesson_ShouldSucceed()
        {
            // Arrange
            var course = await _testsFixture.GetCourse_PendingLessonAsync();
            var enrollment = await _testsFixture.GetActiveCourseForStudentAsync(course.Id);

            // Act
            var response = await _testsFixture.Client.PutAsync(
                $"api/enrollments/{enrollment.EnrollmentId}/complete-lesson/{course.Lessons.First().Id}",
                null);

            var result =
                await _testsFixture.DeserializeResponse<ApiResponse<string>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(result.Success);

            enrollment = await _testsFixture.GetActiveCourseForStudentAsync(course.Id);
            Assert.Equal(100, enrollment.CourseProgress);
        }

        [Fact(DisplayName = "Complete course")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task CompleteCourse_ShouldSucceed()
        {
            // Arrange
            var course = await _testsFixture.GetCourse_PendingFinishAsync();
            var enrollment = await _testsFixture.GetActiveCourseForStudentAsync(course.Id);

            // Act
            var response = await _testsFixture.Client.PutAsync(
                $"api/enrollments/{enrollment.EnrollmentId}/complete-course",
                null);

            var result =
                await _testsFixture.DeserializeResponse<ApiResponse<string>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(result.Success);

            enrollment = await _testsFixture.GetActiveCourseForStudentAsync(course.Id);

            Assert.NotNull(enrollment.CompletionDate);
            Assert.Equal(CourseStatus.Completed, enrollment.CourseStatus);

            response = await _testsFixture.Client
                .GetAsync($"api/enrollments/{enrollment.EnrollmentId}/certificate");

            response.EnsureSuccessStatusCode();

            var certificate =
                await _testsFixture.DeserializeResponse<ApiResponse<CertificateViewModel>>(response);

            Assert.True(certificate.Success);
        }
    }
}
