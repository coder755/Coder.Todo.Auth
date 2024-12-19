using Microsoft.EntityFrameworkCore;

namespace Coder.Todo.Auth.Db;

public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .Property(e => e.Id)
            .HasConversion<byte[]>()
            .HasMaxLength(16);
    }
    
    public DbSet<User> Users { get; set; }
}