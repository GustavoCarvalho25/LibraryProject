using Application.Models;
using MediatR;

namespace Application.Commands.Books.ReturnBookCommand;

public class ReturnBookCommand : IRequest<ResultViewModel>
{
    public Guid BookId { get; set; }
    
    public ReturnBookCommand(Guid bookId)
    {
        BookId = bookId;
    }
}
