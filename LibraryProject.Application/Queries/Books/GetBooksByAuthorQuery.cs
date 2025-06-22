using Application.Models;
using MediatR;

namespace Application.Queries.Books;

public class GetBooksByAuthorQuery : IRequest<ResultViewModel<IEnumerable<BookViewModel>>>
{
    public string AuthorName { get; set; }
    
    public GetBooksByAuthorQuery(string authorName)
    {
        AuthorName = authorName;
    }
}
