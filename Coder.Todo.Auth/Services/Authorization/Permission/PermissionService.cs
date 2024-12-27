﻿using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.Permission;
using EntityFramework.Exceptions.Common;

namespace Coder.Todo.Auth.Services.Authorization.Permission;

public class PermissionService(AuthContext context, ILogger<PermissionService> logger) : IPermissionService
{
    public async Task<Db.Permission> CreatePermissionAsync(string permissionName, string description)
    {
        try
        {
            var permission = new Db.Permission
            {
                Id = Guid.CreateVersion7(),
                Name = permissionName,
                Description = description
            };
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();
            return permission;
        }
        catch (UniqueConstraintException e)
        {
            switch (e.ConstraintName)
            {
                case AuthContext.PermissionNameIndexName:
                    throw new PermissionNameAlreadyExistsException($"Permission {permissionName} already exists");
                default:
                    throw;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating permission");
            throw;
        }
    }
}