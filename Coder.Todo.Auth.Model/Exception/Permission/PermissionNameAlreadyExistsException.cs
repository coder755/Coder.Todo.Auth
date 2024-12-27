namespace Coder.Todo.Auth.Model.Exception.Permission;

public class PermissionNameAlreadyExistsException : System.Exception
{
    public PermissionNameAlreadyExistsException()
    {
    }

    public PermissionNameAlreadyExistsException(string message)
        : base(message)
    {
    }

    public PermissionNameAlreadyExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}