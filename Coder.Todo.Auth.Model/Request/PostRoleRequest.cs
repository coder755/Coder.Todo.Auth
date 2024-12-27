using System.ComponentModel.DataAnnotations;

namespace Coder.Todo.Auth.Model.Request;

public class PostRoleRequest
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}