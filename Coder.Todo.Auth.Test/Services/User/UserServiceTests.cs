using System.Reflection;
using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception;
using Coder.Todo.Auth.Model.Exception.UserValidation;
using Coder.Todo.Auth.Services.User;
using EntityFramework.Exceptions.Common;
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
        var goodData = new Db.User
        {
            UserName = "bestUserName",
            Password = "bestPassword1",
            Email = "best@Email",
            Phone = "2234567890",
        };
        try
        {
            _userService.ValidateUserData(goodData);
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
        Assert.Throws<UserDataValidationException>(
            () => _userService.ValidateUserData(badUserData));
    }
    
    [Test]
    public void CreateUserAsync_ExistingUsername_ThrowsUserNameAlreadyExistsException()
    {
        var uniqueConstraintException = GetUniqueConstraintException(AuthContext.UserNameIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<UserNameAlreadyExistsException>(
            () => _userService.CreateUserAsync(new Db.User()));
    }
    
    [Test]
    public void CreateUserAsync_ExistingEmail_ThrowsEmailAlreadyExistsException()
    {
        var uniqueConstraintException = GetUniqueConstraintException(AuthContext.EmailIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<EmailAlreadyExistsException>(
            () => _userService.CreateUserAsync(new Db.User()));
    }
    
    [Test]
    public void CreateUserAsync_ExistingPhone_ThrowsPhoneNumberAlreadyExistsException()
    {
        var uniqueConstraintException = GetUniqueConstraintException(AuthContext.PhoneIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<PhoneNumberAlreadyExistsException>(
            () => _userService.CreateUserAsync(new Db.User()));
    }
    
    [Test]
    public void CreateUserAsync_ExistingId_ThrowsCreateUserExceptionException()
    {
        var uniqueConstraintException = GetUniqueConstraintException(AuthContext.IdIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<CreateUserException>(
            () => _userService.CreateUserAsync(new Db.User()));
    }

    private static UniqueConstraintException GetUniqueConstraintException(string constraintName)
    {
        var uniqueConstraintException = new UniqueConstraintException();
        var t = uniqueConstraintException.GetType();
        const BindingFlags invokeAtt = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance;
        t.InvokeMember("ConstraintName", invokeAtt, null, uniqueConstraintException, [constraintName]);
        return uniqueConstraintException;
    }
}