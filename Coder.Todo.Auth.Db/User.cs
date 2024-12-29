using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coder.Todo.Auth.Db;

public class User
{
    [Key]
    public required Guid Id { get; init; }
    
    [Required]
    [Column(TypeName = "varchar(50)")]
    public required string UserName { get; init; }

    [Required]
    [Column(TypeName = "binary(32)")]
    public required byte[] PasswordHash { get; init; }
    
    [Required]
    [Column(TypeName = "binary(16)")]
    public required byte[] Salt { get; init; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public required string Email { get; init; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public required string Phone { get; init; }

    [Required]
    // Gets set by Db in AuthContext in OnModelCreating
    public DateTime CreatedDate { get; init; }
}