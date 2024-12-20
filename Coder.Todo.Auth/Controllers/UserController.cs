using System.Net.Mime;
using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception.UserValidation;
using Coder.Todo.Auth.Model.Request;
using Coder.Todo.Auth.Model.Response;
using Coder.Todo.Auth.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Coder.Todo.Auth.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController(IUserService userService, ILogger<UserController> logger)
{
    [HttpPost]
    public async Task<ActionResult<PostUserResponse>> RequestPostUser([FromBody] PostUserRequest req)
    {
        var userToPost = new User()
        {
            UserName = req.Username,
            Password = req.Password,
            Email = req.Email,
            Phone = req.Phone
        };
        try
        {
            var validatedUser = userService.ValidateUser(userToPost);
            var user = await userService.CreateUser(validatedUser);
            var accessToken = userService.CreateAccessToken(user);
            return new PostUserResponse
            {
                AccessToken = accessToken
            };
        }
        catch (UserValidationException e)
        {
            logger.LogError(e, "Error RequestPostUser");
            return new BadRequestResult();
        }
        catch (UserNameAlreadyExistsException e)
        {
            return new BadRequestObjectResult("Username already exists");
        }
        catch (EmailAlreadyExistsException e)
        {
            return new BadRequestObjectResult("Email already exists");
        }
        catch (PhoneNumberAlreadyExistsException e)
        {
            return new BadRequestObjectResult("Phone number already exists");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error RequestPostUser");
            throw new Exception();
        }
    }
    
}