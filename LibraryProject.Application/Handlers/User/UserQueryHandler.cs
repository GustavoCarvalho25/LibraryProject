using Application.Models;
using Application.Queries.Users;
using Application.ViewModels;
using AutoMapper;
using Core.Repository;
using Core.Shared;
using MediatR;

namespace Application.Handlers.User;

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
        try
        {
            // Usar o método paginado do repositório
            var pagedUsers = await _userRepository.GetPagedAsync(
                request.Options,
                null, // sem filtro adicional
                cancellationToken);
            
            // Mapear os resultados
            var userViewModels = _mapper.Map<IEnumerable<UserViewModel>>(pagedUsers.Items);
            
            // Criar o resultado paginado com os ViewModels
            var pagedResult = new PagedResult<UserViewModel>(
                userViewModels,
                pagedUsers.TotalCount,
                pagedUsers.PageNumber,
                pagedUsers.PageSize);
            
            return ResultViewModel<PagedResult<UserViewModel>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return ResultViewModel<PagedResult<UserViewModel>>.Error(ex.Message);
        }
    }

    public async Task<ResultViewModel<UserViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.Id);
        
        if (user == null)
        {
            return ResultViewModel<UserViewModel>.Error($"Usuário com ID {request.Id} não encontrado");
        }
        
        var userViewModel = _mapper.Map<UserViewModel>(user);
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }

    public async Task<ResultViewModel<UserViewModel>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(request.Email);
        
        if (user == null)
        {
            return ResultViewModel<UserViewModel>.Error($"Usuário com email {request.Email} não encontrado");
        }
        
        var userViewModel = _mapper.Map<UserViewModel>(user);
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }
}
