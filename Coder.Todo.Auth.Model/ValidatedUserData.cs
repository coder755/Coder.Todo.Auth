
namespace Coder.Todo.Auth.Model;

public class ValidatedUserData
{
    private string Username_ { get; set; } = null!;
    private byte[] PasswordHash_ { get; set; } = null!;
    private byte[] Salt_ { get; set; } = null!;
    private string Email_ { get; set; } = null!;
    private string Phone_ { get; set; } = null!;
    
    public required string Username
    {
        get => Username_;
        init => Username_ = value;
    }
    
    public required byte[] PasswordHash
    {
        get => PasswordHash_;
        init => PasswordHash_ = value;
    }
    
    public required byte[] Salt
    {
        get => Salt_;
        init => Salt_ = value;
    }
    
    public required string Email
    {
        get => Email_;
        init => Email_ = value;
    }
    
    public required string Phone
    {
        get => Phone_;
        init => Phone_ = value;
    }
}