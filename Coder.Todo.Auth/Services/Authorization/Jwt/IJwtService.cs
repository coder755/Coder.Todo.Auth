namespace Coder.Todo.Auth.Services.Authorization.Jwt;

public interface IJwtService
{
    public string GenerateUserToken(Guid userId);
    public string? GetSubjectFromToken(string token);
}