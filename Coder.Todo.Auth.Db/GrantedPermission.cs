using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

[PrimaryKey(nameof(RoleId), nameof(PermissionId))]
public class GrantedPermission
{
    [Required]
    public Guid PermissionId { get; init; }
    
    [Required]
    public Guid RoleId { get; init; }
}