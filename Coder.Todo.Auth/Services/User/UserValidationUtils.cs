using System.Net.Mail;
using System.Text.RegularExpressions;
using PhoneNumbers;

namespace Coder.Todo.Auth.Services.User;

public static class UserValidationUtils
{
    private static readonly FieldValidationParams UserNameValidationParams = new FieldValidationParams
    {
        Max = 50,
        Min = 8,
        Pattern = "[a-zA-Z0-9_]*$",
        Message = "Username must only contain alphanumeric characters and underscores.",
    };
    
    private static readonly FieldValidationParams PasswordValidationParams = new FieldValidationParams
    {
        Max = 50,
        Min = 8,
        Pattern = @"(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]*$",
        Message = "Password must have at least one letter and one number",
    };
    
    private static readonly FieldValidationParams EmailValidationParams = new FieldValidationParams
    {
        Max = 50,
        Min = 8,
        Pattern = "[a-zA-Z0-9_]*$",
        Message = "Please provide a valid email address.",
    };
    
    public static string ValidateUserName(string userName)
    {
        var match = Regex.IsMatch(userName, UserNameValidationParams.Pattern);
        if (!match) throw new FormatException("Invalid username. " + UserNameValidationParams.Message);
        return userName;
    } 
    
    public static string ValidatePassword(string password)
    {
        var match = Regex.IsMatch(password, PasswordValidationParams.Pattern);
        if (!match) throw new FormatException("Invalid password. " + PasswordValidationParams.Message);
        return password;
    } 
    
    public static string ValidateEmail(string email)
    {
        try
        {
            var formattedEmail = new MailAddress(email);
            return formattedEmail.Address;
        }
        catch
        {
            throw new FormatException("Invalid email. " + EmailValidationParams.Message);
        }
    }

    public static string ValidatePhoneNumber(string phoneNumber)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var phoneNumberParsed = phoneNumberUtil.Parse(phoneNumber, "US");
        var isValid = phoneNumberUtil.IsValidNumber(phoneNumberParsed);
        if (!isValid)
        {
            throw new FormatException("Invalid phone number. Please provide a valid phone number.");
        }
        
        var formattedPhoneNumber = phoneNumberUtil.Format(phoneNumberParsed, PhoneNumberFormat.INTERNATIONAL);
        if (string.IsNullOrEmpty(formattedPhoneNumber))
        {
            throw new FormatException("Invalid phone number. Please provide a valid phone number.");
        }
        return formattedPhoneNumber;
    }
    
    private class FieldValidationParams
    {
        private int Max_ { get; set; }
        private int Min_ { get; set; }
        private string Pattern_ { get; init; } = null!;
        private string Message_ { get; init; } = null!;

        public required int Max
        {
            set => Max_ = value;
        }
        
        public required int Min
        {
            set => Min_ = value;
        }
        
        public required string Pattern
        {
            get
            {
                var lengthRegEx = "^(?=.{" + Min_ + "," + Max_ + "}$)";
                return lengthRegEx + Pattern_;
            }
            init => Pattern_ = value;
        }
        
        public required string Message
        {
            get
            {
                var postMessage = " It must be between " + Min_ + " and " + Max_ + " characters.";
                return Message_ + postMessage;
            }
            init => Message_ = value;
        }
    }
}