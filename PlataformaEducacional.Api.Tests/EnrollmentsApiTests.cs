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
        private readonly IntegrationTestsFixture<Program> _fixture;

        public EnrollmentsApiTests(IntegrationTestsFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Process payment should fail when enrollment does not exist")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task ProcessPayment_ShouldFail_WhenEnrollmentDoesNotExist()
        {
            var enrollmentId = Guid.NewGuid();

            var request = CreatePaymentRequest();

            await _fixture.RegisterNewStudentAsync();
            _fixture.Client.AssignToken(_fixture.Token);

            var response = await _fixture.Client
                .PostAsJsonAsync($"api/enrollments/{enrollmentId}/payment", request);

            var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Matrícula não encontrada.", errors);
        }

        [Fact(DisplayName = "Process payment successfully")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task ProcessPayment_ShouldSucceed()
        {
            var enrollmentId = await _fixture.CreatePendingEnrollmentAsync();

            var request = CreatePaymentRequest();

            var response = await _fixture.Client
                .PostAsJsonAsync($"api/enrollments/{enrollmentId}/payment", request);

            response.EnsureSuccessStatusCode();

            var result =
                await _fixture.DeserializeResponse<ApiResponse<string>>(response);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Perform lesson successfully")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task PerformLesson_ShouldSucceed()
        {
            var course = await _fixture.GetCourse_PendingLessonAsync();

            var enrollment = await _fixture.GetActiveCourseForStudentAsync(course.Id);

            var response = await _fixture.Client.PutAsync(
                $"api/enrollments/{enrollment.EnrollmentId}/complete-lesson/{course.Lessons.First().Id}",
                null);

            response.EnsureSuccessStatusCode();

            var result =
                await _fixture.DeserializeResponse<ApiResponse<string>>(response);

            Assert.True(result.Success);

            enrollment = await _fixture.GetActiveCourseForStudentAsync(course.Id);
            Assert.Equal(100, enrollment.CourseProgress);
        }

        [Fact(DisplayName = "Completed course should already have certificate")]
        [Trait("Category", "API Integration - Enrollment")]
        public async Task CompletedCourse_ShouldHaveCertificate()
        {
            await _fixture.StudentLoginAsync();
            _fixture.Client.AssignToken(_fixture.Token);

            var courseId = await _fixture.GetCourseIdAsync(); // .NET
            var enrollment = await _fixture.GetActiveCourseForStudentAsync(courseId);

            Assert.Equal(CourseStatus.Completed, enrollment.CourseStatus);
            Assert.NotNull(enrollment.CompletionDate);

            var response = await _fixture.Client
                .GetAsync($"api/enrollments/{enrollment.EnrollmentId}/certificate");

            response.EnsureSuccessStatusCode();

            var certificate =
                await _fixture.DeserializeResponse<ApiResponse<CertificateViewModel>>(response);

            Assert.True(certificate.Success);
        }

        private static ProcessPaymentRequest CreatePaymentRequest()
            => new()
            {
                CardName = "Test",
                CardNumber = "4111111111111111",
                CardExpiration = "12/30",
                CardCvv = "123",
                Total = 500
            };
    }
}
