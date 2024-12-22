using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coder.Todo.Auth.Db;

public class Role
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Name { get; init; } = null!;
    
    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Description { get; init; } = null!;
}