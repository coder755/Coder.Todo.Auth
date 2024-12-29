using EntityFramework.Exceptions.MySQL.Pomelo;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

public class AuthContext : DbContext
{
    // User Indices
    public const string UserIdIndexName = "PRIMARY";
    public const string UserNameIndexName = "IX_Users_UserName";
    public const string UserPhoneIndexName = "IX_Users_Phone";
    public const string UserEmailIndexName = "IX_Users_Email";
    
    // Role Indices
    public const string RoleNameIndexName = "IX_Roles_Name";
    
    // Permission Indices
    public const string PermissionNameIndexName = "IX_Permissions_Name";

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
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().HasIndex(u => u.UserName).HasDatabaseName(UserNameIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).HasDatabaseName(UserEmailIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Phone).HasDatabaseName(UserPhoneIndexName).IsUnique();

        // Roles Table
        modelBuilder.Entity<Role>().HasKey(u => u.Id);
        modelBuilder.Entity<Role>().HasIndex(u => u.Name).HasDatabaseName(RoleNameIndexName).IsUnique();
        
        // Permissions Table
        modelBuilder.Entity<Permission>().HasKey(u => u.Id);
        modelBuilder.Entity<Permission>().HasIndex(u => u.Name).HasDatabaseName(PermissionNameIndexName).IsUnique();

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
        
        // Guid binary representation for Roles
        modelBuilder
            .Entity<Role>()
            .Property(e => e.Id)
            .HasConversion<byte[]>()
            .HasMaxLength(16);
        
        // Guid binary representation for Permissions
        modelBuilder
            .Entity<Permission>()
            .Property(e => e.Id)
            .HasConversion<byte[]>()
            .HasMaxLength(16);
        
        // Guid binary representation for GrantedPermissions
        modelBuilder
            .Entity<GrantedPermission>()
            .Property(e => e.PermissionId)
            .HasConversion<byte[]>()
            .HasMaxLength(16);
        modelBuilder
            .Entity<GrantedPermission>()
            .Property(e => e.RoleId)
            .HasConversion<byte[]>()
            .HasMaxLength(16);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<GrantedPermission> GrantedPermissions { get; set; }
}