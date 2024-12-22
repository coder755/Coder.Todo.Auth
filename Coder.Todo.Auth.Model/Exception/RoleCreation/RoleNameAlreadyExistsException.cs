namespace Coder.Todo.Auth.Model.Exception.RoleCreation;

public class RoleNameAlreadyExistsException : System.Exception
{
    public RoleNameAlreadyExistsException()
    {
    }

    public RoleNameAlreadyExistsException(string message)
        : base(message)
    {
    }

    public RoleNameAlreadyExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}