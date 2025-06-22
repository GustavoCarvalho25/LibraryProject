using Application.Commands.Users.AddUserCommand;
using Application.Commands.Users.RemoveUserCommand;
using Application.Commands.Users.UpdateUserCommand;
using Application.Models;
using Application.ViewModels;
using AutoMapper;
using Core.Repository;
using MediatR;

namespace Application.Handlers.User;

public class UserCommandHandler : 
    IRequestHandler<AddUserCommand, ResultViewModel<UserViewModel>>,
    IRequestHandler<UpdateUserCommand, ResultViewModel<UserViewModel>>,
    IRequestHandler<RemoveUserCommand, ResultViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ResultViewModel<UserViewModel>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmail(request.Email);
        if (existingUser != null)
        {
            return ResultViewModel<UserViewModel>.Error($"Já existe um usuário com o email {request.Email}");
        }

        var user = new Core.Entities.User(request.Name, request.Email);
        
        var result = await _userRepository.Add(user);
        
        var userViewModel = _mapper.Map<UserViewModel>(result);
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }

    public async Task<ResultViewModel<UserViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetById(request.Id);
        if (existingUser == null)
        {
            return ResultViewModel<UserViewModel>.Error($"Usuário com ID {request.Id} não encontrado");
        }

        var userWithSameEmail = await _userRepository.GetByEmail(request.Email);
        if (userWithSameEmail != null && userWithSameEmail.Id != request.Id)
        {
            return ResultViewModel<UserViewModel>.Error($"O email {request.Email} já está sendo usado por outro usuário");
        }

        var user = new Core.Entities.User(request.Name, request.Email);
        user.Id = request.Id;

        var updateSuccess = await _userRepository.Update(user);
        
        if (!updateSuccess)
        {
            return ResultViewModel<UserViewModel>.Error("Falha ao atualizar o usuário");
        }
        
        var updatedUser = await _userRepository.GetById(user.Id);
        var userViewModel = _mapper.Map<UserViewModel>(updatedUser);
        
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }

    public async Task<ResultViewModel> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetById(request.Id);
        if (existingUser == null)
        {
            return ResultViewModel.Error($"Usuário com ID {request.Id} não encontrado");
        }

        if (existingUser.Loans != null && existingUser.Loans.Any(l => l.ReturnDate == null))
        {
            return ResultViewModel.Error("Não é possível remover um usuário com empréstimos ativos");
        }

        var removeSuccess = await _userRepository.Remove(existingUser);
        
        if (!removeSuccess)
        {
            return ResultViewModel.Error("Falha ao remover o usuário");
        }
        
        return ResultViewModel.Success();
    }
}
