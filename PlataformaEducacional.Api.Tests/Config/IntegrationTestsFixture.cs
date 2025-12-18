using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using PlataformaEducacional.Api.Requests.Authentication;
using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace PlataformaEducacional.Api.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public string Email = string.Empty;
        public string Password = string.Empty;
        public string Name = string.Empty;
        public string Token = string.Empty;
        public bool Success = false;

        public readonly PlataformaEducacionalAppFactory<TProgram> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            };

            Factory = new PlataformaEducacionalAppFactory<TProgram>();
            Client = Factory.CreateClient(clientOptions);
        }

        public void SetupUserData()
        {
            var faker = new Faker("pt_BR");
            Email = faker.Internet.Email().ToLower();
            Name = faker.Person.FirstName;
            Password = faker.Internet.Password(8, false, "", "@1Ab_");
        }

        public void SaveToken(string token)
        {
            var response = JsonSerializer.Deserialize<ApiResponse<string>>(token,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ApiResponse<string>();

            Token = response.Data!;
            Success = response.Success;
        }

        public async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content!,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                }) ?? throw new InvalidOperationException("Deserialization returned null");
        }

        public async Task<Guid> PendingEnrollmentIdAsync()
        {
            await RegisterNewStudentAsync();
            var courseId = await GetCourseIdAsync();

            var data = new EnrollRequest
            {
                CourseId = courseId
            };

            Client.AssignToken(Token);
            await Client.PostAsJsonAsync("api/students/enroll", data);

            var response = await Client.GetAsync("api/students/pending-courses");
            var result = await DeserializeResponse<ApiResponse<IEnumerable<EnrollmentViewModel>>>(response);
            var course = result!.Data!.FirstOrDefault();

            return course!.EnrollmentId;
        }

        public async Task<EnrollmentViewModel> GetActiveCourseForStudentAsync(Guid courseId)
        {
            await StudentLoginAsync();
            Client.AssignToken(Token);

            var response = await Client.GetAsync("api/students/active-courses");
            var result = await DeserializeResponse<ApiResponse<IEnumerable<EnrollmentViewModel>>>(response);
            var course = result!.Data!.FirstOrDefault(c => c.CourseId == courseId);

            return course!;
        }

        public async Task RegisterNewStudentAsync()
        {
            SetupUserData();

            var request = new RegisterUserRequest
            {
                Email = Email,
                Name = Name,
                Password = Password,
                ConfirmPassword = Password
            };

            var response = await Client.PostAsJsonAsync("/api/authentication/register/student", request);
            response.EnsureSuccessStatusCode();

            SaveToken(await response.Content.ReadAsStringAsync());
        }

        public async Task StudentLoginAsync()
        {
            var request = new LoginUserRequest
            {
                Email = "student@test.com",
                Password = "Test@123"
            };

            var response = await Client.PostAsJsonAsync("/api/authentication/login", request);
            response.EnsureSuccessStatusCode();

            SaveToken(await response.Content.ReadAsStringAsync());
        }

        public async Task AdminLoginAsync()
        {
            var request = new LoginUserRequest
            {
                Email = "admin@test.com",
                Password = "Test@123"
            };

            var response = await Client.PostAsJsonAsync("/api/authentication/login", request);
            response.EnsureSuccessStatusCode();

            SaveToken(await response.Content.ReadAsStringAsync());
        }

        public async Task<Guid> GetCourseIdAsync()
        {
            var response = await Client.GetAsync("api/courses/available-courses");
            var result = await DeserializeResponse<ApiResponse<IEnumerable<CourseViewModel>>>(response);
            var course = result!.Data!.FirstOrDefault(c => c.Name == ".NET");
            return course!.Id;
        }

        public async Task<CourseViewModel> GetCourse_PendingLessonAsync()
        {
            var response = await Client.GetAsync("api/courses/available-courses");
            var result = await DeserializeResponse<ApiResponse<IEnumerable<CourseViewModel>>>(response);
            var course = result!.Data!.FirstOrDefault(c => c.Name == ".NET Core");
            return course!;
        }

        public async Task<CourseViewModel> GetCourse_PendingFinishAsync()
        {
            var response = await Client.GetAsync("api/courses/available-courses");
            var result = await DeserializeResponse<ApiResponse<IEnumerable<CourseViewModel>>>(response);
            var course = result!.Data!.FirstOrDefault(c => c.Name == "Rich Domains");
            return course!;
        }

        public IEnumerable<string> GetErrors(string jsonResponse)
        {
            var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetailsResponse>(
                jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (problemDetails?.Errors == null)
                return Enumerable.Empty<string>();

            if (problemDetails.Errors.TryGetValue("Errors", out var errorMessages))
                return errorMessages ?? Enumerable.Empty<string>();

            return Enumerable.Empty<string>();
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
    }

    public class ValidationProblemDetailsResponse
    {
        [JsonPropertyName("errors")]
        public Dictionary<string, string[]>? Errors { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }
}
