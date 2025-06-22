using Application.Models;
using Application.ViewModels;
using MediatR;

namespace Application.Commands.Users.UpdateUserCommand;

public class UpdateUserCommand : IRequest<ResultViewModel<UserViewModel>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public UpdateUserCommand(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}
