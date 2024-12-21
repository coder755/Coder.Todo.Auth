
namespace Coder.Todo.Auth.Model;

public class ValidatedUserData
{
    private string Username_ { get; set; } = null!;
    private string Password_ { get; set; } = null!;
    private string Email_ { get; set; } = null!;
    private string Phone_ { get; set; } = null!;
    
    public required string Username
    {
        get => Username_;
        set => Username_ = value;
    }
    
    public required string Password
    {
        get => Password_;
        set => Password_ = value;
    }
    
    public required string Email
    {
        get => Email_;
        set => Email_ = value;
    }
    
    public required string Phone
    {
        get => Phone_;
        set => Phone_ = value;
    }
}