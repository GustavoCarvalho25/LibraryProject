using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthor(string authorName)
    => await _context.Books
        .Where(b => b.Author.Contains(authorName))
        .ToListAsync();
}