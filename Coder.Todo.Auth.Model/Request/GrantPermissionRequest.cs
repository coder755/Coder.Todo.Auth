using System.ComponentModel.DataAnnotations;

namespace Coder.Todo.Auth.Model.Request;

public class GrantPermissionRequest
{
    [Required]
    public Guid PermissionId { get; set; }
    [Required]
    public Guid RoleId { get; set; }
}