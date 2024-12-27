namespace Coder.Todo.Auth.Model.Exception.Permission;

public class PermissionDoesNotExistsException : System.Exception
{
    public PermissionDoesNotExistsException()
    {
    }

    public PermissionDoesNotExistsException(string message)
        : base(message)
    {
    }

    public PermissionDoesNotExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}