using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model;
using Coder.Todo.Auth.Model.Exception;
using Coder.Todo.Auth.Model.Exception.UserValidation;
using Coder.Todo.Auth.Services.User;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Coder.Todo.Auth.Test.Services.User;

public class UserServiceTests
{
    private UserService _userService;
    private Mock<AuthContext> _mockAuthContext;
    private Mock<ILogger<UserService>> _mockLogger;

    
    [SetUp]
    public void SetUp()
    {
        var users = new List<Db.User>().AsQueryable();
        _mockAuthContext = new Mock<AuthContext>();
        _mockAuthContext.Setup(c => c.Users).ReturnsDbSet(users);
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockAuthContext.Object, _mockLogger.Object);
    }
    
    [Test]
    public void ValidateUser_GoodData_DoesNotThrow()
    {
        try
        {
            _userService.ValidateUserData("bestUserName", "bestPassword1", "best@Email", "2234567890");
        }
        catch (Exception e)
        {
            Assert.Fail("Expected no exception, but got: " + e.Message);
        }
    }
    
    [Test]
    public void ValidateUser_BadData_ThrowsUserValidationException()
    {
        Assert.Throws<UserDataValidationException>(
            () => _userService.ValidateUserData("", "", "", ""));
    }
    
    [Test]
    public void CreateUserAsync_ExistingUsername_ThrowsUserNameAlreadyExistsException()
    {
        var uniqueConstraintException = ServiceTestUtils.GetUniqueConstraintException(AuthContext.UserNameIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<UserNameAlreadyExistsException>(
            () => _userService.CreateUserAsync(new ValidatedUserData
            {
                Username = "best",
                Password = "user",
                Email = "ever",
                Phone = "123"
            }));
    }
    
    [Test]
    public void CreateUserAsync_ExistingEmail_ThrowsEmailAlreadyExistsException()
    {
        var uniqueConstraintException = ServiceTestUtils.GetUniqueConstraintException(AuthContext.UserEmailIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<EmailAlreadyExistsException>(
            () => _userService.CreateUserAsync(new ValidatedUserData
            {
                Username = "best",
                Password = "user",
                Email = "ever",
                Phone = "123"
            }));
    }
    
    [Test]
    public void CreateUserAsync_ExistingPhone_ThrowsPhoneNumberAlreadyExistsException()
    {
        var uniqueConstraintException = ServiceTestUtils.GetUniqueConstraintException(AuthContext.UserPhoneIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<PhoneNumberAlreadyExistsException>(
            () => _userService.CreateUserAsync(new ValidatedUserData
            {
                Username = "best",
                Password = "user",
                Email = "ever",
                Phone = "123"
            }));
    }
    
    [Test]
    public void CreateUserAsync_ExistingId_ThrowsCreateUserExceptionException()
    {
        var uniqueConstraintException = ServiceTestUtils.GetUniqueConstraintException(AuthContext.UserIdIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<CreateUserException>(
            () => _userService.CreateUserAsync(new ValidatedUserData
            {
                Username = "best",
                Password = "user",
                Email = "ever",
                Phone = "123"
            }));
    }
}