namespace Coder.Todo.Auth.Model.Exception.User;

public class UserDoesNotExistsException : System.Exception
{
    public UserDoesNotExistsException()
    {
    }

    public UserDoesNotExistsException(string message)
        : base(message)
    {
    }

    public UserDoesNotExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}