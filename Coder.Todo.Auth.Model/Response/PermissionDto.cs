namespace Coder.Todo.Auth.Model.Response;

public class PermissionDto
{
    public required Guid Id { get; init; }
    
    public required string Name  { get; init; }
    
    public required string Description  { get; init; }
}