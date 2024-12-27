namespace Coder.Todo.Auth.Model.Response;

public class PostRoleResponse
{
    private RoleDto Role_ { get; set; } = null!;

    public required RoleDto Role
    {
        get => Role_;
        set => Role_ = value;
    }
}