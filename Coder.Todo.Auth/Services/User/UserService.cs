﻿using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Exception;
using Coder.Todo.Auth.Model.Exception.UserValidation;
using EntityFramework.Exceptions.Common;

namespace Coder.Todo.Auth.Services.User;

public class UserService(AuthContext context, ILogger<UserService> logger) : IUserService
{
    public Db.User ValidateUser(Db.User user)
    {
        try
        {
            var formattedUserName = UserValidationUtils.ValidateUserName(user.UserName);
            var formattedPassword = UserValidationUtils.ValidatePassword(user.Password);
            var formattedEmail = UserValidationUtils.ValidateEmail(user.Email);
            var formattedPhoneNumber = UserValidationUtils.ValidatePhoneNumber(user.Phone);
            return new Db.User
            {
                UserName = formattedUserName,
                Password = formattedPassword,
                Email = formattedEmail,
                Phone = formattedPhoneNumber
            };
        }
        catch (FormatException e)
        {
            throw new UserValidationException(e.Message);
        }
    }
    
    public async Task<Db.User> CreateUser(Db.User user)
    {
        try
        {
            user.Id = Guid.CreateVersion7();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        catch (UniqueConstraintException e)
        {
            switch (e.ConstraintName)
            {
                case AuthContext.UserNameIndexName:
                    throw new UserNameAlreadyExistsException();
                case AuthContext.EmailIndexName:
                    throw new EmailAlreadyExistsException();
                case AuthContext.PhoneIndexName:
                    throw new PhoneNumberAlreadyExistsException();
                default:
                    throw new CreateUserException("Unable to save user to database.");
            }
        }
        catch (Exception e)
        {
            logger.LogError("Unable to save user to database: {Message}",  e.Message);
            throw new CreateUserException("Unable to save user to database.");
        }
    }

    public string CreateAccessToken(Db.User user)
    {
        return "";
    }
}