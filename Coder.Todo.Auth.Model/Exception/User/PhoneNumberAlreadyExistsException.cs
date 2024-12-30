namespace Coder.Todo.Auth.Model.Exception.User;

public class PhoneNumberAlreadyExistsException : System.Exception
{
    public PhoneNumberAlreadyExistsException()
    {
    }

    public PhoneNumberAlreadyExistsException(string message)
        : base(message)
    {
    }

    public PhoneNumberAlreadyExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}