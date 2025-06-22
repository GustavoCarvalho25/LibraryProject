using Application.Models;
using MediatR;

namespace Application.Commands.Books;

public class LoanBookCommand : IRequest<ResultViewModel<LoanViewModel>>
{
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    
    public LoanBookCommand(Guid bookId, Guid userId)
    {
        BookId = bookId;
        UserId = userId;
    }
}
