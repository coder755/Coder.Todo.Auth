namespace Coder.Todo.Auth.Model.Exception.Role;

public class RoleDoesNotExistsException : System.Exception
{
    public RoleDoesNotExistsException()
    {
    }

    public RoleDoesNotExistsException(string message)
        : base(message)
    {
    }

    public RoleDoesNotExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}