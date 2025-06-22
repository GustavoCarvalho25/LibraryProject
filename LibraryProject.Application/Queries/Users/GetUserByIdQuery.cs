using Application.Models;
using Application.ViewModels;
using MediatR;

namespace Application.Queries.Users;

public class GetUserByIdQuery : IRequest<ResultViewModel<UserViewModel>>
{
    public Guid Id { get; }
    
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
}
