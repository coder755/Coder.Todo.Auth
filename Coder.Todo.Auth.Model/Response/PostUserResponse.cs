
namespace Coder.Todo.Auth.Model.Response;

public class PostUserResponse
{
    private string AccessToken_ { get; set; } = null!;

    public required string AccessToken
    {
        get => AccessToken_;
        set => AccessToken_ = value;
    }
}