using System.Net.Mime;
using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Request;
using Coder.Todo.Auth.Model.Response;
using Coder.Todo.Auth.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Coder.Todo.Auth.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _logger = logger;
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<ActionResult<PostUserResponse>> RequestPostUser([FromBody] PostUserRequest req)
    {
        try
        {
            _userService.ValidateUserName(req.Username);
            _userService.ValidatePassword(req.Password);
            _userService.ValidateEmail(req.Email);
            _userService.ValidatePhoneNumber(req.Phone);
            var userToPost = new User()
            {
                UserName = req.Username,
                Password = req.Password,
                Email = req.Email,
                Phone = req.Phone
            };
            var user = await _userService.CreateUser(userToPost);
            var accessToken = _userService.CreateAccessToken(user);
            return new PostUserResponse
            {
                AccessToken = accessToken
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error RequestPostUser");
            return new BadRequestResult();
        }
    }
    
}