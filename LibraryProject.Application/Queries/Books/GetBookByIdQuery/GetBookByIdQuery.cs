using Application.Models;
using MediatR;

namespace Application.Queries.Books.GetBookByIdQuery;

public class GetBookByIdQuery : IRequest<ResultViewModel<IEnumerable<BookViewModel>>>
{
    public Guid Id { get; private set; }

    public GetBookByIdQuery(Guid id)
    {
        Id = id;
    }
}