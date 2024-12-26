
// ReSharper disable ClassNeverInstantiated.Global

using System.ComponentModel.DataAnnotations;

namespace Coder.Todo.Auth.Model.Request;

public class PostUserRequest
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Phone { get; set; } = null!;
}