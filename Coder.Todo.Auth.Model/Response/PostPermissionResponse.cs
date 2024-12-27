namespace Coder.Todo.Auth.Model.Response;

public class PostPermissionResponse
{
    private PermissionDto Permission_ { get; set; } = null!;

    public required PermissionDto Permission
    {
        get => Permission_;
        set => Permission_ = value;
    }
}