using Application.Models;
using MediatR;

namespace Application.Commands.Users.RemoveUserCommand;

public class RemoveUserCommand : IRequest<ResultViewModel>
{
    public Guid Id { get; set; }
    
    public RemoveUserCommand(Guid id)
    {
        Id = id;
    }
}
