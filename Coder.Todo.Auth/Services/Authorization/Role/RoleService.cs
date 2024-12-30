using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.Permission;
using Coder.Todo.Auth.Model.Exception.Role;
using Coder.Todo.Auth.Services.Authorization.Permission;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Services.Authorization.Role;

public class RoleService(
    AuthContext context,
    IPermissionService permissionService,
    ILogger<RoleService> logger) : IRoleService
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

    public async Task<Db.Role> GetRoleByNameAsync(string roleName)
    {
        try
        {
            var role = await context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
            {
                throw new RoleDoesNotExistsException();
            }
            return role;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Db.Role> GrantPermission(string roleName, string permissionName)
    {
        try
        {
            var role = await GetRoleByNameAsync(roleName);
            if (role is null)
            {
                throw new RoleDoesNotExistsException();
            }
            var permission = await permissionService.GetPermissionAsync(permissionName);
            if (permission is null)
            {
                throw new PermissionDoesNotExistsException();
            }

            if (role.Permissions.Any(p => p.Name == permissionName))
            {
                return role;
            }
            role.Permissions.Add(permission);
            await context.SaveChangesAsync();
            return role;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error granting permission");
            throw;
        }
    }
}