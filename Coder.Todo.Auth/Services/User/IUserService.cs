namespace Coder.Todo.Auth.Services.User;

public interface IUserService
{
    Db.User ValidateUser(Db.User user);
    Task<Db.User> CreateUser(Db.User user);
    string CreateAccessToken(Db.User user);
}