using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Domain;

using System.Security.Claims;

namespace PlataformaEducacional.Api.Extensions
{
    public class AppIdentityUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AppIdentityUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetUserId()
        {
            if (!IsAuthenticated()) return string.Empty;

            var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return claim ?? string.Empty;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }
    }
}
