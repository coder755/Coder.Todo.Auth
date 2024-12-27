

namespace Coder.Todo.Auth.Model.Exception;

public class CreateUserException : System.Exception
{
    public CreateUserException()
    {
    }

    public CreateUserException(string message)
        : base(message)
    {
    }

    public CreateUserException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}