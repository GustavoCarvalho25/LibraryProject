using Application.Models;
using Application.ViewModels;
using MediatR;

namespace Application.Commands.Books.LoanBookCommand;

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
