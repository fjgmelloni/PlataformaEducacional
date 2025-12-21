using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PlataformaEducacional.Api.Requests.Authentication;
using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;
using PlataformaEducacional.StudentAdministration.Data;
using PlataformaEducacional.StudentAdministration.Domain;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlataformaEducacional.Api.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection
        : ICollectionFixture<IntegrationTestsFixture<Program>>
    { }

    public class IntegrationTestsFixture<TProgram> : IDisposable
        where TProgram : class
    {
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Token { get; private set; } = string.Empty;
        public bool Success { get; private set; }

        public readonly PlataformaEducacionalAppFactory<TProgram> Factory;
        public HttpClient Client { get; }

        public IntegrationTestsFixture()
        {
            Factory = new PlataformaEducacionalAppFactory<TProgram>();
            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            });
        }

        public void SetupUserData()
        {
            var faker = new Faker("pt_BR");

            Email = faker.Internet.Email().ToLowerInvariant();
            Name = faker.Person.FirstName;
            Password = faker.Internet.Password(
                10,
                false,
                @"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).*"
            );
        }

        public void SaveToken(string json)
        {
            var response = JsonSerializer.Deserialize<ApiResponse<string>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (response is null || string.IsNullOrWhiteSpace(response.Data))
                throw new InvalidOperationException("Token inválido retornado pela API.");

            Token = response.Data;
            Success = response.Success;
        }

        public async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(
                       content,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true,
                           Converters = { new JsonStringEnumConverter() }
                       }
                   )
                   ?? throw new InvalidOperationException("Erro ao desserializar resposta da API.");
        }

        public async Task RegisterNewStudentAsync()
        {
            SetupUserData();
            Client.SetJsonMediaType();

            var request = new RegisterUserRequest
            {
                Email = Email,
                Name = Name,
                Password = Password,
                ConfirmPassword = Password
            };

            var response = await Client.PostAsJsonAsync(
                "/api/authentication/register/student",
                request
            );

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            SaveToken(json);

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(Token);

            var userId = Guid.Parse(
                jwt.Claims.First(c => c.Type == "sub").Value
            );

            using var scope = Factory.Services.CreateScope();

            var studentContext =
                scope.ServiceProvider.GetRequiredService<StudentAdministrationContext>();

            if (!studentContext.Students.Any(s => s.Id == userId))
            {
                var student = new Student(userId, Name);
                studentContext.Students.Add(student);
                await studentContext.SaveChangesAsync();
            }
        }


        public async Task<Guid> CreateCourseAsync()
        {
            await AdminLoginAsync();
            Client.AssignToken(Token);

            var request = new
            {
                Name = $"Course Test {Guid.NewGuid()}",
                ContentDescription = "Test Course",
                Workload = 10,
                Available = true,
                Price = 100
            };

            var response = await Client.PostAsJsonAsync("api/courses", request);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Failed to create test course");

            var result =
                await DeserializeResponse<ApiResponse<string>>(response);

            return Guid.Parse(result.Data!);
        }



        public async Task StudentLoginAsync()
        {
            using var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            });

            client.SetJsonMediaType();

            var request = new LoginUserRequest
            {
                Email = "student@test.com",
                Password = "Teste@123"
            };

            var response = await client.PostAsJsonAsync(
                "/api/authentication/login",
                request
            );

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"Student login failed. Status: {response.StatusCode}");

            SaveToken(await response.Content.ReadAsStringAsync());
        }



        public async Task AdminLoginAsync()
        {
            using var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            });

            client.SetJsonMediaType();

            var request = new LoginUserRequest
            {
                Email = "admin@test.com",
                Password = "Teste@123"
            };

            var response = await client.PostAsJsonAsync(
                "/api/authentication/login",
                request
            );

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"Admin login failed. Status: {response.StatusCode}");

            SaveToken(await response.Content.ReadAsStringAsync());
        }


        public async Task<Guid> GetCourseIdAsync()
        {
            var response = await Client.GetAsync("api/courses/available-courses");
            response.EnsureSuccessStatusCode();

            var result =
                await DeserializeResponse<ApiResponse<IEnumerable<CourseViewModel>>>(response);

            var course = result.Data!.FirstOrDefault(c => c.Name == ".NET")
                         ?? throw new InvalidOperationException("Curso '.NET' não encontrado.");

            return course.Id;
        }

        public async Task<CourseViewModel> GetCourse_PendingLessonAsync()
        {
            var response = await Client.GetAsync("api/courses/available-courses");
            response.EnsureSuccessStatusCode();

            var result =
                await DeserializeResponse<ApiResponse<IEnumerable<CourseViewModel>>>(response);

            return result.Data!.First(c => c.Name == ".NET Core");
        }

        public async Task<CourseViewModel> GetCourse_PendingFinishAsync()
        {
            var response = await Client.GetAsync("api/courses/available-courses");
            response.EnsureSuccessStatusCode();

            var result =
                await DeserializeResponse<ApiResponse<IEnumerable<CourseViewModel>>>(response);

            return result.Data!.First(c => c.Name == "Rich Domains");
        }

        public async Task<Guid> CreatePendingEnrollmentAsync()
        {
            await RegisterNewStudentAsync();
            Client.AssignToken(Token);

            var courseId = await GetCourseIdAsync();

            await Client.PostAsJsonAsync(
                "api/students/enroll",
                new EnrollRequest { CourseId = courseId }
            );

            var response = await Client.GetAsync("api/students/pending-courses");
            response.EnsureSuccessStatusCode();

            var result =
                await DeserializeResponse<ApiResponse<IEnumerable<EnrollmentViewModel>>>(response);

            return result.Data!.First().EnrollmentId;
        }

        public async Task<(CourseViewModel course, EnrollmentViewModel enrollment)>
            CreateActiveEnrollmentWithPendingLessonAsync()
        {
            var course = await GetCourse_PendingLessonAsync();

            await StudentLoginAsync();
            Client.AssignToken(Token);

            var enrollment = await GetActiveCourseForStudentAsync(course.Id);

            return (course, enrollment);
        }

        public async Task<EnrollmentViewModel> GetActiveCourseForStudentAsync(Guid courseId)
        {
            var response = await Client.GetAsync("api/students/active-courses");
            response.EnsureSuccessStatusCode();

            var result =
                await DeserializeResponse<ApiResponse<IEnumerable<EnrollmentViewModel>>>(response);

            return result.Data!.First(c => c.CourseId == courseId);
        }

        public IEnumerable<string> GetErrors(string jsonResponse)
        {
            var details = JsonSerializer.Deserialize<ValidationProblemDetailsResponse>(
                jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (details?.Errors is null)
                return Enumerable.Empty<string>();

            return details.Errors.SelectMany(e => e.Value);
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
