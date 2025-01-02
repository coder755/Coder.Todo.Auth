using System.Net.Mime;
using Asp.Versioning;
using Coder.Todo.Auth.Model.Dto;
using Coder.Todo.Auth.Model.Exception.User;
using Coder.Todo.Auth.Model.Request;
using Coder.Todo.Auth.Model.Response;
using Coder.Todo.Auth.Services.Authorization;
using Coder.Todo.Auth.Services.Authorization.Jwt;
using Coder.Todo.Auth.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coder.Todo.Auth.Controllers;

[ApiController]
[ControllerName("User")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController(
    IUserService userService, 
    IJwtService jwtService, 
    IAuthHeaderProvider authHeaderProvider, 
    ILogger<UserController> logger)
{
    [HttpPost]
    public async Task<ActionResult<PostUserResponse>> PostUserAsync([FromBody] PostUserRequest req)
    {
        try
        {
            var validatedUserData = userService.ValidateUserData(req.Username, req.Password, req.Email, req.Phone);
            var user = await userService.CreateUserAsync(validatedUserData);
            var accessToken = jwtService.GenerateUserToken(user.Id);
            return new PostUserResponse
            {
                AccessToken = accessToken
            };
        }
        catch (UserDataValidationException e)
        {
            return new BadRequestObjectResult(e.Message);
        }
        catch (UserNameAlreadyExistsException)
        {
            return new BadRequestObjectResult("Username already exists");
        }
        catch (EmailAlreadyExistsException)
        {
            return new BadRequestObjectResult("Email already exists");
        }
        catch (PhoneNumberAlreadyExistsException)
        {
            return new BadRequestObjectResult("Phone number already exists");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error PostUserAsync");
            return new BadRequestObjectResult("Error Posting User");
        }
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "User")]
    public async Task<ActionResult<UserDto>> GetUserAsync()
    {
        var userId = authHeaderProvider.GetUserIdFromToken();
        if (userId.Equals(Guid.Empty))
        {
            return new UnauthorizedResult();
        }

        try
        {
            var user = await userService.GetUserAsync(userId);
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
            };
            return userDto;
        }
        catch (UserDoesNotExistsException)
        {
            return new UnauthorizedResult();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error GetUserAsync");
            return new BadRequestObjectResult("Error GetUserAsync");
        }
    }
}