using Application.Commands.Books.LoanBookCommand;
using Application.Commands.Books.ReturnBookCommand;
using Application.Models;
using Application.ViewModels;
using AutoMapper;
using Core.Repository;
using MediatR;

namespace Application.Handlers.LoanHandlers;

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
        if (book is null)
            return ResultViewModel<LoanViewModel>.Error($"Book with ID {request.BookId} not found");
        
        if (!book.IsAvailable)
            return ResultViewModel<LoanViewModel>.Error("Book is not available for loan");
        
        var user = await _userRepository.GetById(request.UserId);
        if (user is null)
            return ResultViewModel<LoanViewModel>.Error($"User with ID {request.UserId} not found");

        var loan = book.LoanTo(user);
        
        var result = await _loanRepository.Add(loan);
        
        if (result is null)
            return ResultViewModel<LoanViewModel>.Error("Failed to create loan record");
        
        var updateSuccess = await _bookRepository.Update(book);

        if (!updateSuccess)
        {
            await _loanRepository.Remove(loan);
            return ResultViewModel<LoanViewModel>.Error("Failed to update book status");
        }
        
        var loanViewModel = _mapper.Map<LoanViewModel>(loan);
            
        return ResultViewModel<LoanViewModel>.Success(loanViewModel);
    }

    public async Task<ResultViewModel> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.BookId);
        if (book is null)
            return ResultViewModel.Error($"Book with ID {request.BookId} not found");
        
        if (book.IsAvailable)
            return ResultViewModel.Error("This book is already available and cannot be returned");
        
        var activeLoan = await _loanRepository.GetActiveLoanByBookId(request.BookId);
        if (activeLoan == null)
            return ResultViewModel.Error("No active loan found for this book");

        activeLoan.Return();
        book.Return();
            
        var bookUpdateSuccess = await _bookRepository.Update(book);
        var loanUpdateSuccess = await _loanRepository.Update(activeLoan);
        
        if (!bookUpdateSuccess || !loanUpdateSuccess)
            return ResultViewModel.Error("Failed to update book or loan status");

        return ResultViewModel.Success();
    }
}
