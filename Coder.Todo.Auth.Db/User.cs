using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(50)")]
    public string UserName { get; init; } = null!;

    [Required]
    [Column(TypeName = "binary(32)")]
    public byte[] PasswordHash { get; init; } = null!;
    
    [Required]
    [Column(TypeName = "binary(16)")]
    public byte[] Salt { get; init; } = null!;

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Email { get; init; } = null!;

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Phone { get; init; } = null!;

    [Required]
    public DateTime CreatedDate { get; init; }
}