using Coder.Todo.Auth.Db;

namespace Coder.Todo.Auth.Services.Authorization;

public interface IRoleService
{
    Task<Role> CreateRoleAsync(string roleName, string description);
}