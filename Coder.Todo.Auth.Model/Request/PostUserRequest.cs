
namespace Coder.Todo.Auth.Model.Request;

public class PostUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    PostUserRequest(string username, string password, string email, string phone)
    {
        Username = username;
        Password = password;
        Email = email;
        Phone = phone;
    }
}