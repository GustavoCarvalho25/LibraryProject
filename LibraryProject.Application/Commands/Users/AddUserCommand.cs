using Application.Models;
using Application.ViewModels;
using MediatR;

namespace Application.Commands.Users;

public class AddUserCommand : IRequest<ResultViewModel<UserViewModel>>
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    public AddUserCommand(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
