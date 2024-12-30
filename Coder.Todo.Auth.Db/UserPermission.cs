using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

[PrimaryKey(nameof(UserId), nameof(PermissionId))]
public class UserPermission
{
    [Required]
    public required Guid UserId { get; init; }
    [Required]
    public required Guid PermissionId { get; init; }
}