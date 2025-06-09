using Core.Entities;

namespace Core.Repository;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksByAuthor(string authorName);
}