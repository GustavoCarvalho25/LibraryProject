using Application.Models;
using MediatR;

namespace Application.Queries.Books;

public class GetBookByIdQuery : IRequest<ResultViewModel<BookViewModel>>
{
    public Guid Id { get; set; }
    
    public GetBookByIdQuery(Guid id)
    {
        Id = id;
    }
}
