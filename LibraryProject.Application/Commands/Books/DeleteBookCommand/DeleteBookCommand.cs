using Application.Models;
using MediatR;

namespace Application.Commands.Books.DeleteBookCommand;

public class DeleteBookCommand : IRequest<ResultViewModel<BookViewModel>>
{
    public Guid Id { get; set; }
    
    public DeleteBookCommand(Guid id)
    {
        Id = id;
    }
}