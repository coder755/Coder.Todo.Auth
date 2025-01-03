﻿using System.Net.Mime;
using Asp.Versioning;
using Coder.Todo.Auth.Model.Exception.Permission;
using Coder.Todo.Auth.Model.Exception.Role;
using Coder.Todo.Auth.Model.Request;
using Coder.Todo.Auth.Model.Response;
using Coder.Todo.Auth.Services.Authorization.Permission;
using Coder.Todo.Auth.Services.Authorization.Role;
using Microsoft.AspNetCore.Mvc;

namespace Coder.Todo.Auth.Controllers;

[ApiController]
[ControllerName("Admin")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class AdminController(
    IRoleService roleService, 
    IPermissionService permissionService, 
    ILogger<AdminController> logger)
{
    [HttpPost("role")]
    public async Task<ActionResult<PostRoleResponse>> PostRoleAsync([FromBody] PostRoleRequest req)
    {
        try
        {
            var role = await roleService.CreateRoleAsync(req.Name, req.Description);
            return new PostRoleResponse
            {
                Role = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                }
            };
        }
        catch (RoleNameAlreadyExistsException)
        {
            return new BadRequestObjectResult("Role name already exists");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error PostRoleAsync");
            return new BadRequestObjectResult("Error Posting Role");
        }
    }
    
    [HttpPost("role/{roleName}/permission")]
    public async Task<ActionResult> GrantPermissionAsync([FromRoute] string roleName, [FromBody] GrantPermissionRequest req)
    {
        try
        {
            await roleService.GrantPermission(roleName, req.PermissionName);
            return new OkResult();
        }
        catch (RoleDoesNotExistsException)
        {
            return new BadRequestObjectResult("Role does not exist");
        }
        catch (PermissionDoesNotExistsException)
        {
            return new BadRequestObjectResult("Permission does not exist");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error GrantPermissionAsync");
            return new BadRequestObjectResult("Error Granting Permission");
        }
    }
    
    [HttpPost("permission")]
    public async Task<ActionResult<PostPermissionResponse>> PostPermissionAsync([FromBody] PostPermissionRequest req)
    {
        try
        {
            var permission = await permissionService.CreatePermissionAsync(req.Name, req.Description);
            return new PostPermissionResponse
            {
                Permission = new PermissionDto
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    Description = permission.Description
                }
            };
        }
        catch (PermissionNameAlreadyExistsException)
        {
            return new BadRequestObjectResult("Permission already exists");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error PostPermissionAsync");
            return new BadRequestObjectResult("Error Posting Permission");
        }
    }
}