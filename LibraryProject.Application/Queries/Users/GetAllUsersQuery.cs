using Application.Models;
using Application.ViewModels;
using Core.Shared;
using MediatR;

namespace Application.Queries.Users;

public class GetAllUsersQuery : IRequest<ResultViewModel<PagedResult<UserViewModel>>>
{
    public QueryOptions Options { get; }
    
    public GetAllUsersQuery(QueryOptions? options = null)
    {
        Options = options ?? new QueryOptions();
    }
}
