

using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception;

namespace Coder.Todo.Auth.Services.User;

public class UserService : IUserService
{
    private readonly AuthContext _context;
    private readonly ILogger<UserService> _logger;
    
    public UserService( AuthContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Db.User> CreateUser(Db.User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to save user to database {Message}",  e.Message);
            throw new CreateUserException("Unable to save user to database.");
        }
    }

    public void ValidateUserName(string username)
    {
        
    }

    public void ValidatePassword(string password)
    {
        
    }

    public void ValidateEmail(string email)
    {
        
    }

    public void ValidatePhoneNumber(string phone)
    {
        
    }

    public string CreateAccessToken(Db.User user)
    {
        return "";
    }
}