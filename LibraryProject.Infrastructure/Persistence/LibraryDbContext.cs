using Core.Entities;
using Core.Events;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class LibraryDbContext : DbContext
{
    private readonly IDomainEventPublisher? _domainEventPublisher;
    
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
    
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) {}
    
    public LibraryDbContext(
        DbContextOptions<LibraryDbContext> options,
        IDomainEventPublisher domainEventPublisher) : base(options)
    {
        _domainEventPublisher = domainEventPublisher;
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();
            
        var result = await base.SaveChangesAsync(cancellationToken);
        
        if (result > 0 && _domainEventPublisher != null && entitiesWithEvents.Any())
        {
            foreach (var entity in entitiesWithEvents)
            {
                await _domainEventPublisher.PublishEventsAsync(entity.DomainEvents, cancellationToken);
                entity.ClearDomainEvents();
            }
        }
        
        return result;
    }
    
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

            e.OwnsOne(b => b.ISBN, isbn =>
            {
                isbn.Property(i => i.Value).HasColumnName("ISBN");
            });
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