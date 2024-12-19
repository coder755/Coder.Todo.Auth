namespace Coder.Todo.Auth.Services.User;

public interface IUserService
{
    Task<Db.User> CreateUser(Db.User user);
    void ValidateUserName(string username);
    void ValidatePassword(string password);
    void ValidateEmail(string email);
    void ValidatePhoneNumber(string phone);
    string CreateAccessToken(Db.User user);
}