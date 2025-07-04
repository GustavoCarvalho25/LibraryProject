using Application.Models;
using Application.Queries.Users;
using Application.ViewModels;
using AutoMapper;
using Core.Repository;
using Core.Shared;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Application.Handlers.UserHandlers;

public class UserQueryHandler : 
    IRequestHandler<GetAllUsersQuery, ResultViewModel<PagedResult<UserViewModel>>>,
    IRequestHandler<GetUserByIdQuery, ResultViewModel<UserViewModel>>,
    IRequestHandler<GetUserByEmailQuery, ResultViewModel<UserViewModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ResultViewModel<PagedResult<UserViewModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetPagedAsync(request.Options);
        
        if(users.Items.IsNullOrEmpty())
            return ResultViewModel<PagedResult<UserViewModel>>.Error("Any entity founded");
            
        var userViewModels = _mapper.Map<IEnumerable<UserViewModel>>(users.Items);
            
        var result = PagedResult<UserViewModel>.From(userViewModels, users);
            
        return ResultViewModel<PagedResult<UserViewModel>>.Success(result);
    }

    public async Task<ResultViewModel<UserViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.Id);
        
        if (user == null)
            return ResultViewModel<UserViewModel>.Error($"User with ID {request.Id} not founded");
        
        var userViewModel = _mapper.Map<UserViewModel>(user);
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }

    public async Task<ResultViewModel<UserViewModel>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(request.Email);
        
        if (user is null)
            return ResultViewModel<UserViewModel>.Error($"User with email {request.Email} not founded");
        
        var userViewModel = _mapper.Map<UserViewModel>(user);
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }
}
