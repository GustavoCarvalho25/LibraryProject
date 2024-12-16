using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class LibraryDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
    
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) {}
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(s => s.Id);
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(b => b.Id);
        });

        modelBuilder.Entity<Loan>(e =>
        {
            e.HasKey(l => l.Id);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}