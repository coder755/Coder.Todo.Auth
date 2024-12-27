namespace Coder.Todo.Auth.Model.Exception.GrantedPermission;

public class GrantedPermissionExistsException : System.Exception
{
    public GrantedPermissionExistsException()
    {
    }

    public GrantedPermissionExistsException(string message)
        : base(message)
    {
    }

    public GrantedPermissionExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}