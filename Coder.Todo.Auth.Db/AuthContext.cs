using EntityFramework.Exceptions.MySQL.Pomelo;
using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

public class AuthContext : DbContext
{
    private const string IdIndexName = "IX_Users_Id";
    public const string UserNameIndexName = "IX_Users_UserName";
    public const string PhoneIndexName = "IX_Users_Phone";
    public const string EmailIndexName = "IX_Users_Email";

    public AuthContext()
    {
    }

    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Indices built here to take advantage of UniqueConstraintException
        modelBuilder.Entity<User>().HasIndex(u => u.Id).HasDatabaseName(IdIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.UserName).HasDatabaseName(UserNameIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).HasDatabaseName(EmailIndexName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Phone).HasDatabaseName(PhoneIndexName).IsUnique();
        
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
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
    
    public DbSet<User> Users { get; set; }
}