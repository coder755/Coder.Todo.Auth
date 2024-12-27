using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.GrantedPermission;
using Coder.Todo.Auth.Model.Exception.Permission;
using Coder.Todo.Auth.Model.Exception.Role;
using Coder.Todo.Auth.Services.Authorization.Role;
using EntityFramework.Exceptions.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Coder.Todo.Auth.Test.Services.Authorization.Role;

public class RoleServiceTests
{
    private RoleService _roleService;
    private Mock<AuthContext> _mockAuthContext;
    private Mock<ILogger<RoleService>> _mockLogger;
    private readonly Guid _nullRoleId = Guid.Empty;
    private readonly Guid _nullPermissionId = Guid.Empty;
    private Db.Role _validRole;
    private Db.Permission _validPermission;
    
    [SetUp]
    public void SetUp()
    {
        _validRole = new Db.Role { Id = Guid.NewGuid() };
        _validPermission = new Db.Permission { Id = Guid.NewGuid() };
        var roles = new List<Db.Role> { _validRole };
        var permissions = new List<Db.Permission> { _validPermission };
        var grantedPermissions = new List<Db.GrantedPermission>().AsQueryable();
        _mockAuthContext = new Mock<AuthContext>();
        _mockAuthContext.Setup(c => c.Roles).ReturnsDbSet(roles.AsQueryable());
        _mockAuthContext.Setup(c => c.Permissions).ReturnsDbSet(permissions.AsQueryable());
        _mockAuthContext.Setup(c => c.GrantedPermissions).ReturnsDbSet(grantedPermissions);
        
        _mockAuthContext
            .Setup(ac => ac.Roles.FindAsync(It.Is<Guid>(s => s == _validRole.Id)))
            .Returns(new ValueTask<Db.Role?>(_validRole));
        _mockAuthContext
            .Setup(ac => ac.Roles.FindAsync(It.Is<Guid>(s => s == _nullRoleId)))
            .Returns(new ValueTask<Db.Role?>());
        _mockAuthContext
            .Setup(ac => ac.Permissions.FindAsync(It.Is<Guid>(s => s == _nullPermissionId)))
            .Returns(new ValueTask<Db.Permission?>());
        _mockAuthContext
            .Setup(ac => ac.Permissions.FindAsync(It.Is<Guid>(s => s == _validPermission.Id)))
            .Returns(new ValueTask<Db.Permission?>(_validPermission));
        
        _mockLogger = new Mock<ILogger<RoleService>>();
        _roleService = new RoleService(_mockAuthContext.Object, _mockLogger.Object);
    }
    
    [Test]
    public async Task CreateRoleAsync_GoodData_DoesNotThrow()
    {
        try
        {
            await _roleService.CreateRoleAsync("bestRole", "bestDescription");
        }
        catch (Exception e)
        {
            Assert.Fail("Expected no exception, but got: " + e.Message);
        }
    }
    
    [Test]
    public void CreateRoleAsync_ExistingRole_ThrowsRoleNameAlreadyExistsException()
    {
        var uniqueConstraintException = ServiceTestUtils.GetUniqueConstraintException(AuthContext.RoleNameIndexName);
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(uniqueConstraintException);
        Assert.ThrowsAsync<RoleNameAlreadyExistsException>(
            () => _roleService.CreateRoleAsync("bestName", "bestDescription"));
    }

    [Test]
    public async Task GrantPermission_GoodData_DoesNotThrow()
    {
        try
        {
            await _roleService.GrantPermission(_validRole.Id, _validPermission.Id);
        }
        catch (Exception e)
        {
            Assert.Fail("Expected no exception, but got: " + e.Message);
        }
    }
    
    [Test]
    public void GrantPermission_RoleDoesNotExist_ThrowsRoleDoesNotExistsException()
    {
        Assert.ThrowsAsync<RoleDoesNotExistsException>(
            () => _roleService.GrantPermission(_nullRoleId, _validPermission.Id));
    }
    
    [Test]
    public void GrantPermission_PermissionDoesNotExist_ThrowsPermissionDoesNotExistsException()
    {
        Assert.ThrowsAsync<PermissionDoesNotExistsException>(
            () => _roleService.GrantPermission(_validRole.Id, _nullPermissionId));
    }
    
    [Test]
    public void GrantPermission_ExistingGrantedPermission_ThrowsGrantedPermissionExistsException()
    {
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .Throws(new UniqueConstraintException());
        Assert.ThrowsAsync<GrantedPermissionExistsException>(
            () => _roleService.GrantPermission(_validRole.Id, _validPermission.Id));
    }
}