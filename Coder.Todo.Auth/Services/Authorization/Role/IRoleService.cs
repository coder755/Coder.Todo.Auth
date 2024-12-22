namespace Coder.Todo.Auth.Services.Authorization.Role;

public interface IRoleService
{
    Task<Db.Role> CreateRoleAsync(string roleName, string description);
}