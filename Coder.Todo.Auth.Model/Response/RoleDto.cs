namespace Coder.Todo.Auth.Model.Response;

public class RoleDto
{
    private Guid Id_ { get; set; }
    private string Name_ { get; set; } = null!;
    private string Description_ { get; set; } = null!;

    public required Guid Id
    {
        get => Id_;
        set => Id_ = value;
    }
    
    public required string Name
    {
        get => Name_;
        set => Name_ = value;
    }
    
    public required string Description
    {
        get => Description_;
        set => Description_ = value;
    }
}