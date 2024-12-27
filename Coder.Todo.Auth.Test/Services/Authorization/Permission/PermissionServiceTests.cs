using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.Permission;
using Coder.Todo.Auth.Services.Authorization.Permission;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Coder.Todo.Auth.Test.Services.Authorization.Permission;

public class PermissionServiceTests
{
    private PermissionService _permissionService;
    private Mock<AuthContext> _mockAuthContext;
    private Mock<ILogger<PermissionService>> _mockLogger;
    
    [SetUp]
    public void SetUp()
    {
        var permissions = new List<Db.Permission>().AsQueryable();
        _mockAuthContext = new Mock<AuthContext>();
        _mockAuthContext.Setup(c => c.Permissions).ReturnsDbSet(permissions);
        _mockLogger = new Mock<ILogger<PermissionService>>();
        _permissionService = new PermissionService(_mockAuthContext.Object, _mockLogger.Object);
    }
    
    [Test]
    public async Task CreatePermissionAsync_GoodData_DoesNotThrow()
    {
        try
        {
            await _permissionService.CreatePermissionAsync("bestPermission", "does things");
        }
        catch (Exception e)
        {
            Assert.Fail("Expected no exception, but got: " + e.Message);
        }
    }
    
    [Test]
    public void CreatePermissionAsync_ExistingPermission_ThrowsPermissionNameAlreadyExistsException()
    {
        var uniqueConstraintException = ServiceTestUtils.GetUniqueConstraintException(AuthContext.PermissionNameIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<PermissionNameAlreadyExistsException>(
            () => _permissionService.CreatePermissionAsync("bestName", "does things"));
    }
}