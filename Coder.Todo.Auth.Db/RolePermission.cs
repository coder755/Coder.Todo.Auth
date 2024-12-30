using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

[PrimaryKey(nameof(RoleId), nameof(PermissionId))]
public class RolePermission
{
    [Required]
    public required Guid RoleId { get; init; }
    [Required]
    public required Guid PermissionId { get; init; }
}