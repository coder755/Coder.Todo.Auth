using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

public class AuthContext : DbContext
{
    public AuthContext()
    {
    }

    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    
    public DbSet<User> Users { get; set; }
}