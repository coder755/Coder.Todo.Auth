namespace Coder.Todo.Auth.Services.Authorization.Role;

public interface IRoleService
{
    Task<Db.Role> CreateRoleAsync(string roleName, string description);
    Task<Db.Role> GrantPermission(string roleName, string permissionName);
}