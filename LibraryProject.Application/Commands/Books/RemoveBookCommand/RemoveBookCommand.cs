using Application.Models;
using MediatR;

namespace Application.Commands.Books;

public class RemoveBookCommand : IRequest<ResultViewModel>
{
    public Guid Id { get; set; }
    
    public RemoveBookCommand(Guid id)
    {
        Id = id;
    }
}
