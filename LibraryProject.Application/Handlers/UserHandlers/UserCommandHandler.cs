using Application.Commands.Users.AddUserCommand;
using Application.Commands.Users.RemoveUserCommand;
using Application.Commands.Users.UpdateUserCommand;
using Application.Models;
using Application.ViewModels;
using AutoMapper;
using Core.Entities;
using Core.Repository;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Application.Handlers.UserHandlers;

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
            return ResultViewModel<UserViewModel>.Error($"Already exists a user with this email: {request.Email}");

        var user = new User(request.Name, request.Email);
        
        var result = await _userRepository.Add(user);
        
        if (result is null)
            return ResultViewModel<UserViewModel>.Error("Failed to add entity");
        
        var userViewModel = _mapper.Map<UserViewModel>(result);
        
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }

    public async Task<ResultViewModel<UserViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.Id);
        if (user is null)
            return ResultViewModel<UserViewModel>.Error($"User with ID {request.Id} not found");
        
        var userWithSameEmail = await _userRepository.GetByEmail(request.Email);
        if (userWithSameEmail is not null && userWithSameEmail.Id != request.Id)
            return ResultViewModel<UserViewModel>.Error($"The email {request.Email} is already in use by another user");
        
        user.Update(request.Name,request.Email);

        var updateSuccess = await _userRepository.Update(user);
        
        if (!updateSuccess)
            return ResultViewModel<UserViewModel>.Error("Failed to update entity");
        
        var userViewModel = _mapper.Map<UserViewModel>(user);
        
        return ResultViewModel<UserViewModel>.Success(userViewModel);
    }

    public async Task<ResultViewModel> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.Id);
        if (user is null)
            return ResultViewModel.Error($"User with ID {request.Id} not found");

        if (!user.Loans.IsNullOrEmpty() && user.Loans.Any(l => l.ReturnDate == null))
            return ResultViewModel.Error("It is not possible to remove a user with active loans");
        
        user.Remove();
        
        var removeSuccess = await _userRepository.Update(user);
        
        if (!removeSuccess)
            return ResultViewModel.Error("Failed to remove entity");
        
        return ResultViewModel.Success();
    }
}
