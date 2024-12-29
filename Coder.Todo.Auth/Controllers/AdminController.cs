using System.Net.Mime;
using Coder.Todo.Auth.Model.Exception.GrantedPermission;
using Coder.Todo.Auth.Model.Exception.Permission;
using Coder.Todo.Auth.Model.Exception.Role;
using Coder.Todo.Auth.Model.Request;
using Coder.Todo.Auth.Model.Response;
using Coder.Todo.Auth.Services.Authorization.Permission;
using Coder.Todo.Auth.Services.Authorization.Role;
using Microsoft.AspNetCore.Mvc;

namespace Coder.Todo.Auth.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
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
        catch (RoleNameAlreadyExistsException e)
        {
            return new BadRequestObjectResult(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error PostRoleAsync");
            return new BadRequestObjectResult("Error Posting Role");
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
        catch (RoleNameAlreadyExistsException e)
        {
            return new BadRequestObjectResult(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error PostPermissionAsync");
            return new BadRequestObjectResult("Error Posting Permission");
        }
    }

    [HttpPost("permission/grant")]
    public async Task<ActionResult> GrantPermissionAsync([FromBody] GrantPermissionRequest req)
    {
        try
        {
            await roleService.GrantPermission(req.RoleId, req.PermissionId);
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
        catch (GrantedPermissionExistsException)
        {
            return new BadRequestObjectResult("Granted permission already exists");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error GrantPermissionAsync");
            return new BadRequestObjectResult("Error Granting Permission");
        }
    }
}