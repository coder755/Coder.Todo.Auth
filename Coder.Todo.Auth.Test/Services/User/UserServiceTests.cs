using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.UserValidation;
using Coder.Todo.Auth.Services.User;
using Microsoft.Extensions.Logging;
using Moq;

namespace Coder.Todo.Auth.Test.Services.User;

public class UserServiceTests
{
    private UserService _userService;
    private Mock<AuthContext> _mockAuthContext;
    private Mock<ILogger<UserService>> _mockLogger;
    
    [SetUp]
    public void SetUp()
    {
        _mockAuthContext = new Mock<AuthContext>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockAuthContext.Object, _mockLogger.Object);
    }
    
    [Test]
    public void ValidateUser_GoodData_DoesNotThrow()
    {
        var goodData = new Db.User
        {
            UserName = "bestUserName",
            Password = "bestPassword1",
            Email = "best@Email",
            Phone = "2234567890",
        };
        try
        {
            _userService.ValidateUser(goodData);
        }
        catch (Exception e)
        {
            Assert.Fail("Expected no exception, but got: " + e.Message);
        }
    }
    
    [Test]
    public void ValidateUser_BadData_ThrowsUserValidationException()
    {
        var badUserData = new Db.User { UserName = "" };
        Assert.Throws<UserValidationException>(
            () => _userService.ValidateUser(badUserData));
    }
}