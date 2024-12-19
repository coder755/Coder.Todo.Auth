
// ReSharper disable ClassNeverInstantiated.Global
namespace Coder.Todo.Auth.Model.Request;

public class PostUserRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
}