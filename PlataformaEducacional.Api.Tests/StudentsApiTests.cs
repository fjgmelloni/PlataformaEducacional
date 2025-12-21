using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.Api.Tests.Config;
using System.Net;
using System.Net.Http.Json;

namespace PlataformaEducacional.Api.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class StudentsApiTests
    {
        private readonly IntegrationTestsFixture<Program> _fixture;

        public StudentsApiTests(IntegrationTestsFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Enroll student already enrolled in the course")]
        public async Task EnrollStudent_ShouldFail_WhenStudentDoesNotExist()
        {
            var courseId = await _fixture.GetCourseIdAsync();

            await _fixture.StudentLoginAsync();
            _fixture.Client.AssignToken(_fixture.Token);

            var response = await _fixture.Client.PostAsJsonAsync(
                "api/students/enroll",
                new EnrollRequest { CourseId = courseId }
            );

            var errors = _fixture.GetErrors(
                await response.Content.ReadAsStringAsync()
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Aluno não encontrado.", errors);
        }



        [Fact(DisplayName = "Enroll student successfully")]
        public async Task EnrollStudent_ShouldSucceed()
        {
            await _fixture.RegisterNewStudentAsync();
            _fixture.Client.AssignToken(_fixture.Token);

            var courseId = await _fixture.CreateCourseAsync();

            var response = await _fixture.Client.PostAsJsonAsync(
                "api/students/enroll",
                new EnrollRequest { CourseId = courseId }
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
