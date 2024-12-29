
namespace Coder.Todo.Auth.Model;

public class ValidatedUserData
{
    public required string Username { get; init; }
    
    public required byte[] PasswordHash { get; init; }
    public required byte[] Salt { get; init; }
    
    public required string Email { get; init; }
    
    public required string Phone { get; init; }
}