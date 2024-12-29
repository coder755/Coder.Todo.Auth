namespace Coder.Todo.Auth.Model.Dto;

public class UserDto
{
    public required Guid Id { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
}