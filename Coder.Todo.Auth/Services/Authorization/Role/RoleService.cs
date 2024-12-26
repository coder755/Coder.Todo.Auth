using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.GrantedPermission;
using Coder.Todo.Auth.Model.Exception.Permission;
using Coder.Todo.Auth.Model.Exception.Role;
using EntityFramework.Exceptions.Common;

namespace Coder.Todo.Auth.Services.Authorization.Role;

public class RoleService(AuthContext context, ILogger<RoleService> logger) : IRoleService
{
    public async Task<Db.Role> CreateRoleAsync(string roleName, string description)
    {
        try
        {
            var role = new Db.Role
            {
                Id = Guid.CreateVersion7(),
                Name = roleName,
                Description = description
            };
            context.Roles.Add(role);
            await context.SaveChangesAsync();
            return role;
        }
        catch (UniqueConstraintException e)
        {
            switch (e.ConstraintName)
            {
                case AuthContext.RoleNameIndexName:
                    throw new RoleNameAlreadyExistsException($"Role {roleName} already exists");
                default:
                    logger.LogError(e, "Error creating role");
                    throw;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating role");
            throw;
        }
    }

    public async Task<GrantedPermission> GrantPermission(Guid roleId, Guid permissionId)
    {
        try
        {
            var role = await context.Roles.FindAsync(roleId);
            if (role is null)
            {
                throw new RoleDoesNotExistsException();
            }
            var permission = await context.Permissions.FindAsync(permissionId);
            if (permission is null)
            {
                throw new PermissionDoesNotExistsException();
            }
            var grantedPermission = new GrantedPermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id
            };
            
            context.GrantedPermissions.Add(grantedPermission);
            await context.SaveChangesAsync();
            return grantedPermission;
        }
        catch (UniqueConstraintException e)
        {
            throw new GrantedPermissionExistsException();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error granting permission");
            throw;
        }
    }
}