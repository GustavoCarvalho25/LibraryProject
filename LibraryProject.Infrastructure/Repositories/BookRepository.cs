using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class BookRepository : IBaseRepository<Book>
{
    private readonly LibraryDbContext _context;
    
    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }
    
    public async Task<Book> Post(Book entity)
    {
        await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        return entity;
    }
}