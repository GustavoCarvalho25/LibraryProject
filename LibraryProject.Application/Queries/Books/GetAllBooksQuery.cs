using Application.Models;
using Core.Shared;
using MediatR;

namespace Application.Queries.Books;

public class GetAllBooksQuery : IRequest<ResultViewModel<PagedResult<BookViewModel>>>
{
    public QueryOptions Options { get; }
    
    public GetAllBooksQuery(QueryOptions? options = null)
    {
        Options = options ?? new QueryOptions();
    }
}
