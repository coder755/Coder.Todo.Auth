﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

[Index(nameof(Id))]
public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Phone { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }
}