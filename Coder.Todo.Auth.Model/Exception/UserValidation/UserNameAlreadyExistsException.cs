namespace Coder.Todo.Auth.Model.Exception.UserValidation;

public class UserNameAlreadyExistsException : System.Exception
{
    public UserNameAlreadyExistsException()
    {
    }

    public UserNameAlreadyExistsException(string message)
        : base(message)
    {
    }

    public UserNameAlreadyExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}