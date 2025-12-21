using System.Net;
using System.Net.Http.Json;
using PlataformaEducacional.Api.Requests.Authentication;
using PlataformaEducacional.Api.Tests.Config;
using Xunit;

namespace PlataformaEducacional.Api.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class AuthenticationApiTests
    {
        private readonly IntegrationTestsFixture<Program> _fixture;

        public AuthenticationApiTests(IntegrationTestsFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Register student should return token")]
        [Trait("Category", "API - Authentication")]
        public async Task RegisterStudent_ShouldReturnToken()
        {
            _fixture.SetupUserData();

            using var client = _fixture.Factory.CreateClient();

            var request = new RegisterUserRequest
            {
                Email = _fixture.Email,
                Name = _fixture.Name,
                Password = _fixture.Password,
                ConfirmPassword = _fixture.Password
            };

            var response = await client.PostAsJsonAsync(
                "/api/authentication/register/student",
                request
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            _fixture.SaveToken(await response.Content.ReadAsStringAsync());

            Assert.True(_fixture.Success);
            Assert.False(string.IsNullOrWhiteSpace(_fixture.Token));
        }

        [Fact(DisplayName = "Login should return token")]
        [Trait("Category", "API - Authentication")]
        public async Task Login_ShouldReturnToken()
        {
            using var client = _fixture.Factory.CreateClient();

            var request = new LoginUserRequest
            {
                Email = "admin@test.com",
                Password = "Teste@123"
            };

            var response = await client.PostAsJsonAsync(
                "/api/authentication/login",
                request
            );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            _fixture.SaveToken(await response.Content.ReadAsStringAsync());

            Assert.True(_fixture.Success);
            Assert.False(string.IsNullOrWhiteSpace(_fixture.Token));
        }
    }
}
