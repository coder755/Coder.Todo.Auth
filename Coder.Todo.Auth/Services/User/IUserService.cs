using Coder.Todo.Auth.Model;

namespace Coder.Todo.Auth.Services.User;

public interface IUserService
{
    ValidatedUserData ValidateUserData(string username, string password, string email, string phone);
    Task<Db.User> CreateUserAsync(ValidatedUserData validatedUserData);
    string CreateAccessToken(Db.User user);
}