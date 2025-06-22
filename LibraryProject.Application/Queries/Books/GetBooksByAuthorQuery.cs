using Application.Models;
using Core.Shared;
using MediatR;

namespace Application.Queries.Books;

public class GetBooksByAuthorQuery : IRequest<ResultViewModel<PagedResult<BookViewModel>>>
{
    public string AuthorName { get; }
    public QueryOptions Options { get; }
    
    public GetBooksByAuthorQuery(string authorName, QueryOptions? options = null)
    {
        AuthorName = authorName;
        Options = options ?? new QueryOptions();
    }
}
