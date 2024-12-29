namespace Coder.Todo.Auth.Model.Exception.UserValidation;

public class UserDataValidationException : System.Exception
{
    public UserDataValidationException()
    {
    }

    public UserDataValidationException(string message)
        : base(message)
    {
    }

    public UserDataValidationException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}