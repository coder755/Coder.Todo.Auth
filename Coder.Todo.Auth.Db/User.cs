using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

[Index(nameof(Id))]
public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(50)")]
    public string UserName { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Password { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Email { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Phone { get; set; } = null!;

    [Required]
    public DateTime CreatedDate { get; set; }
}