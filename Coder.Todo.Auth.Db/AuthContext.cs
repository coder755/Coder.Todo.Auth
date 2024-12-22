﻿using EntityFramework.Exceptions.MySQL.Pomelo;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

public class AuthContext : DbContext
{
    // User Indices
    public const string UserIdIndexName = "IX_Users_Id";
    public const string UserNameIndexName = "IX_Users_UserName";
    public const string UserPhoneIndexName = "IX_Users_Phone";
    public const string UserEmailIndexName = "IX_Users_Email";
    
    // Role Indices
    private const string RoleIdIndexName = "IX_Roles_Id";
    public const string RoleNameIndexName = "IX_Roles_Name";
    public AuthContext()
    {
    }

    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Indices built here to take advantage of UniqueConstraintException
        // Users Table
        modelBuilder.Entity<User>().HasIndex(u => u.Id).HasDatabaseName(UserIdIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.UserName).HasDatabaseName(UserNameIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).HasDatabaseName(UserEmailIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Phone).HasDatabaseName(UserPhoneIndexName).IsUnique();

        // Roles Table
        modelBuilder.Entity<Role>().HasIndex(u => u.Id).HasDatabaseName(RoleIdIndexName).IsUnique();
        modelBuilder.Entity<Role>().HasIndex(u => u.Name).HasDatabaseName(RoleNameIndexName).IsUnique();

        // Guid binary representation
        modelBuilder
            .Entity<User>()
            .Property(e => e.Id)
            .HasConversion<byte[]>()
            .HasMaxLength(16);
        
        // Auto set time createdDate with timestamp
        modelBuilder
            .Entity<User>()
            .Property(e => e.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        // Guid binary representation
        modelBuilder
            .Entity<Role>()
            .Property(e => e.Id)
            .HasConversion<byte[]>()
            .HasMaxLength(16);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
}