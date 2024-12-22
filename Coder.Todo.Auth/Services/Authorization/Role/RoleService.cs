using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.RoleCreation;
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
}