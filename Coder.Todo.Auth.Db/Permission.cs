using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coder.Todo.Auth.Db;

public class Permission
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(50)")]
    public required string Name { get; init; }
    
    [Required]
    [Column(TypeName = "varchar(50)")]
    public required string Description { get; init; }
}