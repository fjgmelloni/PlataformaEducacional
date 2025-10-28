using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlataformaEducacional.Api.Extensions;
using PlataformaEducacional.Api.Requests.Authentication;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PlataformaEducacional.Api.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            ICurrentUser identityUser,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtSettings> jwtSettings
        ) : base(notifications, mediatorHandler, identityUser)
        {
            _mediatorHandler = mediatorHandler;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register/student")]
        public async Task<ActionResult> RegisterStudent(RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await RegisterUser(request, "STUDENT");

            if (!IsOperationValid())
                return CustomResponse();

            var command = new AddStudentCommand(result, request.Name!);
            await _mediatorHandler.SendCommandAsync(command);

            if (!IsOperationValid())
                return CustomResponse();

            var token = await GenerateJwt(request.Email);
            return CustomResponse(HttpStatusCode.Created, token);
        }

        [HttpPost("register/admin")]
        public async Task<ActionResult> RegisterAdmin(RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await RegisterUser(request, "ADMIN");

            if (!IsOperationValid())
                return CustomResponse();

            var token = await GenerateJwt(request.Email);
            return CustomResponse(HttpStatusCode.Created, token);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserRequest login)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, true);

            if (result.Succeeded)
            {
                var token = await GenerateJwt(login.Email);
                return CustomResponse(HttpStatusCode.OK, token);
            }

            NotifyError("Login", "E-mail ou senha inválidos.");
            return CustomResponse();
        }

        private async Task<string> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return string.Empty;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Name, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()),
                new(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret!);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<Guid> RegisterUser(RegisterUserRequest request, string role)
        {
            var userIdentity = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userIdentity, request.Password!);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userIdentity, role);
            }
            else
            {
                foreach (var error in result.Errors)
                    NotifyError("Identity", error.Description);
            }

            return result.Succeeded ? Guid.Parse(userIdentity.Id) : Guid.Empty;
        }

        private static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
