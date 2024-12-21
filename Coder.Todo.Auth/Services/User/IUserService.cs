namespace Coder.Todo.Auth.Services.User;

public interface IUserService
{
    Db.User ValidateUserData(Db.User user);
    Task<Db.User> CreateUserAsync(Db.User user);
    string CreateAccessToken(Db.User user);
}