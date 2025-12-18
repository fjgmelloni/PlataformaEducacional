using PlataformaEducacional.Api.Requests.Course;
using PlataformaEducacional.Api.Tests.Config;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;
using System.Net;
using System.Net.Http.Json;

namespace PlataformaEducacional.Api.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class CoursesApiTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public CoursesApiTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "List available courses for enrollment")]
        [Trait("Category", "API Integration - Course")]
        public async Task GetAvailableCourses_ShouldReturnSuccess()
        {
            // Act
            var response = await _testsFixture.Client
                .GetAsync("api/courses/available-courses");

            response.EnsureSuccessStatusCode();

            var courses =
                await _testsFixture.DeserializeResponse<
                    ApiResponse<IEnumerable<CourseViewModel>>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(courses!.Data!.Any());
        }

        [Fact(DisplayName = "Add course")]
        [Trait("Category", "API Integration - Course")]
        public async Task AddCourse_ShouldReturnSuccess()
        {
            // Arrange
            var request = new AddCourseRequest
            {
                Name = "Course ABC",
                ContentDescription = "Description",
                Workload = 350,
                Available = true,
                Price = 500
            };

            await _testsFixture.AdminLoginAsync();
            _testsFixture.Client.AssignToken(_testsFixture.Token);

            // Act
            var response = await _testsFixture.Client
                .PostAsJsonAsync("api/courses", request);

            response.EnsureSuccessStatusCode();

            var result =
                await _testsFixture.DeserializeResponse<ApiResponse<string>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Add lesson to course")]
        [Trait("Category", "API Integration - Course")]
        public async Task AddLesson_ShouldReturnSuccess()
        {
            // Arrange
            await _testsFixture.AdminLoginAsync();
            _testsFixture.Client.AssignToken(_testsFixture.Token);

            var courseId = await _testsFixture.GetCourseIdAsync();

            var lessonRequest = new AddLessonRequest
            {
                CourseId = courseId,
                Content = "Lesson content",
                Material = "Lesson material",
                Order = 6,
                Title = "Lesson 6"
            };

            // Act
            var response = await _testsFixture.Client
                .PostAsJsonAsync("api/courses/lesson", lessonRequest);

            response.EnsureSuccessStatusCode();

            var result =
                await _testsFixture.DeserializeResponse<ApiResponse<string>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.True(result.Success);
        }
    }
}
