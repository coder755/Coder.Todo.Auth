namespace Coder.Todo.Auth.Services.Authorization.Permission;

public interface IPermissionService
{
    Task<Db.Permission> CreatePermissionAsync(string permissionName, string description);
    Task<Db.Permission?> GetPermissionAsync(string permissionName);
}