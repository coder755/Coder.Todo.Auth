
namespace Coder.Todo.Auth.Model;

public class User
{
    private Guid ExternalIdP { get; set; }
    private string UsernameP { get; set; } = null!;
    private string EmailP { get; set; } = null!;
    private string PhoneP { get; set; } = null!;
    
    public required Guid ExternalId
    {
        get => ExternalIdP;
        set => ExternalIdP = value;
    }
    
    public required string Username
    {
        get => UsernameP;
        set => UsernameP = value;
    }
    
    public required string Email
    {
        get => EmailP;
        set => EmailP = value;
    }
    
    public required string Phone
    {
        get => PhoneP;
        set => PhoneP = value;
    }
}