using Coder.Todo.Auth.Services.Authorization.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Coder.Todo.Auth.Services.Authorization;

public class AuthHeaderProvider(IHttpContextAccessor httpContextAccessor, IJwtService jwtService) : ControllerBase, IAuthHeaderProvider
{
    public Guid GetUserIdFromToken()
    {
        var authHeader = httpContextAccessor.HttpContext?.Request?.Headers.Authorization.FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var sub = jwtService.GetSubjectFromToken(token);
            return sub != null ? Guid.Parse(sub) : Guid.Empty;
        }
        return Guid.Empty;
    }
}