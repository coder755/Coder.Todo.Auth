using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model;
using Coder.Todo.Auth.Model.Exception;
using Coder.Todo.Auth.Model.Exception.UserValidation;
using EntityFramework.Exceptions.Common;

namespace Coder.Todo.Auth.Services.User;

public class UserService(AuthContext context, ILogger<UserService> logger) : IUserService
{
    public ValidatedUserData ValidateUserData(string username, string password, string email, string phone)
    {
        try
        {
            var formattedUserName = UserValidationUtils.ValidateUserName(username);
            var formattedPassword = UserValidationUtils.ValidatePassword(password);
            var formattedEmail = UserValidationUtils.ValidateEmail(email);
            var formattedPhoneNumber = UserValidationUtils.ValidatePhoneNumber(phone);
            return new ValidatedUserData
            {
                Username = formattedUserName,
                Password = formattedPassword,
                Email = formattedEmail,
                Phone = formattedPhoneNumber
            };
        }
        catch (FormatException e)
        {
            throw new UserDataValidationException(e.Message);
        }
    }
    
    public async Task<Db.User> CreateUserAsync(ValidatedUserData validatedUserData)
    {
        try
        {
            var user = new Db.User
            {
                Id = Guid.CreateVersion7(),
                UserName = validatedUserData.Username,
                Password = validatedUserData.Password,
                Email = validatedUserData.Email,
                Phone = validatedUserData.Phone
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        catch (UniqueConstraintException e)
        {
            switch (e.ConstraintName)
            {
                case AuthContext.UserNameIndexName:
                    throw new UserNameAlreadyExistsException();
                case AuthContext.EmailIndexName:
                    throw new EmailAlreadyExistsException();
                case AuthContext.PhoneIndexName:
                    throw new PhoneNumberAlreadyExistsException();
                default:
                    throw new CreateUserException("Unable to save user to database.");
            }
        }
        catch (Exception e)
        {
            logger.LogError("Unable to save user to database: {Message}",  e.Message);
            throw new CreateUserException("Unable to save user to database.");
        }
    }

    public string CreateAccessToken(Db.User user)
    {
        return "";
    }
}