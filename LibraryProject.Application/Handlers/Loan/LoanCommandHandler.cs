using Application.Commands.Books;
using Application.Models;
using AutoMapper;
using Core.Entities;
using Core.Repository;
using MediatR;

namespace Application.Handlers;

public class LoanCommandHandler : 
    IRequestHandler<LoanBookCommand, ResultViewModel<LoanViewModel>>,
    IRequestHandler<ReturnBookCommand, ResultViewModel>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly IMapper _mapper;

    public LoanCommandHandler(
        IBookRepository bookRepository,
        IUserRepository userRepository,
        ILoanRepository loanRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _loanRepository = loanRepository;
        _mapper = mapper;
    }

    public async Task<ResultViewModel<LoanViewModel>> Handle(LoanBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.BookId);
        if (book == null)
        {
            return ResultViewModel<LoanViewModel>.Error($"Book with ID {request.BookId} not found.");
        }

        var user = await _userRepository.GetById(request.UserId);
        if (user == null)
        {
            return ResultViewModel<LoanViewModel>.Error($"User with ID {request.UserId} not found.");
        }

        var loan = book.LoanTo(user);
            
        var updateSuccess = await _bookRepository.Update(book);
        
        if (!updateSuccess)
        {
            return ResultViewModel<LoanViewModel>.Error("Failed to update book status.");
        }
            
        var result = await _loanRepository.Add(loan);
            
        if (result == null)
        {
            return ResultViewModel<LoanViewModel>.Error("Failed to create loan record.");
        }
            
        var loanViewModel = _mapper.Map<LoanViewModel>(loan);
            
        return ResultViewModel<LoanViewModel>.Success(loanViewModel);
    }

    public async Task<ResultViewModel> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.BookId);
        if (book == null)
        {
            return ResultViewModel.Error($"Book with ID {request.BookId} not found.");
        }

        book.Return();
            
        var updateSuccess = await _bookRepository.Update(book);
        
        if (!updateSuccess)
        {
            return ResultViewModel.Error("Failed to update book status.");
        }

        return ResultViewModel.Success();
    }
}
