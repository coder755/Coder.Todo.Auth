using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole
{
    [Required]
    public required Guid UserId { get; init; }
    [Required]
    public required Guid RoleId { get; init; }
}