using System.ComponentModel.DataAnnotations;

namespace Coder.Todo.Auth.Model.Request;

public class GrantPermissionRequest
{
    [Required]
    public required string PermissionName { get; set; }
    [Required]
    public required string RoleName { get; set; }
}