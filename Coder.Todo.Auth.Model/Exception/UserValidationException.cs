namespace Coder.Todo.Auth.Model.Exception;

public class UserValidationException : System.Exception
{
    public UserValidationException()
    {
    }

    public UserValidationException(string message)
        : base(message)
    {
    }

    public UserValidationException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}