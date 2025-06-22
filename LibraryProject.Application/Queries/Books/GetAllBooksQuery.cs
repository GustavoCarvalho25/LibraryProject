using Application.Models;
using MediatR;

namespace Application.Queries.Books;

public class GetAllBooksQuery : IRequest<ResultViewModel<IEnumerable<BookViewModel>>>
{
    
}
