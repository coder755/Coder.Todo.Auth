
namespace Coder.Todo.Auth.Model.Response;

public class PostUserResponse
{
    private string AccessTokenP { get; set; } = null!;

    public required string AccessToken
    {
        get => AccessTokenP;
        set => AccessTokenP = value;
    }
}