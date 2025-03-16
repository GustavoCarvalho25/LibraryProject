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
            e.HasMany(u => u.Loans)
                .WithOne(l => l.Customer)
                .HasForeignKey(l => l.CustomerId);
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(b => b.Id);
            e.HasMany(b => b.Loans)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId);
        });

        modelBuilder.Entity<Loan>(e =>
        {
            e.HasKey(l => l.Id);
            e.HasOne(l => l.Customer)
                .WithMany(c => c.Loans)
                .HasForeignKey(l => l.CustomerId);
            e.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}