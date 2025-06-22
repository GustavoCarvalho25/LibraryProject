using Application.Models;
using Application.ViewModels;
using MediatR;

namespace Application.Queries.Users;

public class GetUserByEmailQuery : IRequest<ResultViewModel<UserViewModel>>
{
    public string Email { get; }
    
    public GetUserByEmailQuery(string email)
    {
        Email = email;
    }
}
