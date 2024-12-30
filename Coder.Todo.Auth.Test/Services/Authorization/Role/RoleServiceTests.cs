using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.Permission;
using Coder.Todo.Auth.Model.Exception.Role;
using Coder.Todo.Auth.Services.Authorization.Permission;
using Coder.Todo.Auth.Services.Authorization.Role;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Coder.Todo.Auth.Test.Services.Authorization.Role;

public class RoleServiceTests
{
    private RoleService _roleService;
    private Mock<AuthContext> _mockAuthContext;
    private Mock<IPermissionService> _mockPermissionService;
    private const string NullRoleName = "null";
    private const string NullPermissionName = "null";
    private Db.Role _validRole;
    private Db.Permission _validPermission;
    
    [SetUp]
    public void SetUp()
    {
        _validRole = new Db.Role { Id = Guid.NewGuid(), Name = "best", Description = "best" };
        _validPermission = new Db.Permission { Id = Guid.NewGuid(), Name = "admin", Description = "admin" };
        var roles = new List<Db.Role> { _validRole };
        var permissions = new List<Db.Permission> { _validPermission };
        var rolePermissions = new List<RolePermission>().AsQueryable();
        _mockAuthContext = new Mock<AuthContext>();
        _mockAuthContext.Setup(c => c.Roles).ReturnsDbSet(roles.AsQueryable());
        _mockAuthContext.Setup(c => c.Permissions).ReturnsDbSet(permissions.AsQueryable());
        _mockAuthContext.Setup(c => c.RolePermissions).ReturnsDbSet(rolePermissions);
        
        _mockAuthContext
            .Setup(ac => ac.Roles.FindAsync(It.Is<string>(s => s == _validRole.Name)))
            .Returns(new ValueTask<Db.Role?>(_validRole));
        _mockAuthContext
            .Setup(ac => ac.Roles.FindAsync(It.Is<string>(s => s == NullRoleName)))
            .Returns(new ValueTask<Db.Role?>());
        _mockAuthContext
            .Setup(ac => ac.Permissions.FindAsync(It.Is<string>(s => s == NullPermissionName)))
            .Returns(new ValueTask<Db.Permission?>());
        _mockAuthContext
            .Setup(ac => ac.Permissions.FindAsync(It.Is<Guid>(s => s == _validPermission.Id)))
            .Returns(new ValueTask<Db.Permission?>(_validPermission));
        
        _mockPermissionService = new Mock<IPermissionService>();
        var mockRoleLogger = new Mock<ILogger<RoleService>>();
        _roleService = new RoleService(_mockAuthContext.Object, _mockPermissionService.Object, mockRoleLogger.Object);
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
        _mockPermissionService
            .Setup(ps => ps.GetPermissionAsync(_validPermission.Name))
            .ReturnsAsync(_validPermission);
        try
        {
            await _roleService.GrantPermission(_validRole.Name, _validPermission.Name);
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
            () => _roleService.GrantPermission(NullRoleName, _validPermission.Name));
    }
    
    [Test]
    public void GrantPermission_PermissionDoesNotExist_ThrowsPermissionDoesNotExistsException()
    {
        Assert.ThrowsAsync<PermissionDoesNotExistsException>(
            () => _roleService.GrantPermission(_validRole.Name, NullPermissionName));
    }
    
    [Test]
    public async Task GrantPermission_ExistingRolePermission_DoesNotAddToContext()
    {
        var permisionList = new List<Db.Permission>
        {
            _validPermission
        };
        var roleWithPermission = new Db.Role
        {
            Id = Guid.NewGuid(),
            Name = "roleWithPermission",
            Description = "bestDescription",
            Permissions = permisionList
        };
        var data = new List<Db.Role> { roleWithPermission };
        _mockAuthContext
            .Setup(ac => ac.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);
        _mockAuthContext
            .Setup(ac => ac.Roles)
            .ReturnsDbSet(data);
        _mockPermissionService
            .Setup(ps => ps.GetPermissionAsync(_validPermission.Name))
            .ReturnsAsync(_validPermission);
        
        var role = await _roleService.GrantPermission(roleWithPermission.Name, _validPermission.Name);
        _mockAuthContext.Verify(
            authContext => authContext.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        Assert.That(roleWithPermission.Id, Is.EqualTo(role.Id));
    }
}