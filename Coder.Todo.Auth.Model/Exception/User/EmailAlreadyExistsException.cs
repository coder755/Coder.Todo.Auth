﻿namespace Coder.Todo.Auth.Model.Exception.User;

public class EmailAlreadyExistsException : System.Exception
{
    public EmailAlreadyExistsException()
    {
    }

    public EmailAlreadyExistsException(string message)
        : base(message)
    {
    }

    public EmailAlreadyExistsException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}