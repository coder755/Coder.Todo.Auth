namespace Coder.Todo.Auth.Services.Authorization;

public interface IAuthHeaderProvider
{
    Guid GetUserIdFromToken();
}