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

        try
        {
            // Usar o método de domínio para emprestar o livro
            var loan = book.LoanTo(user);
            
            // Atualizar o livro no repositório (agora está marcado como não disponível)
            await _bookRepository.Update(book);
            
            // Salvar o empréstimo
            await _loanRepository.Add(loan);
            
            // Mapear para o ViewModel
            var loanViewModel = new LoanViewModel(
                loan.Id,
                loan.CustomerId,
                user.Name,
                loan.BookId,
                book.Title,
                loan.LoanDate
            );
            
            return ResultViewModel<LoanViewModel>.Success(loanViewModel);
        }
        catch (InvalidOperationException ex)
        {
            return ResultViewModel<LoanViewModel>.Error(ex.Message);
        }
    }

    public async Task<ResultViewModel> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.BookId);
        if (book == null)
        {
            return ResultViewModel.Error($"Book with ID {request.BookId} not found.");
        }

        try
        {
            // Usar o método de domínio para devolver o livro
            book.Return();
            
            // Atualizar o livro no repositório (agora está marcado como disponível)
            await _bookRepository.Update(book);
            
            return ResultViewModel.Success();
        }
        catch (InvalidOperationException ex)
        {
            return ResultViewModel.Error(ex.Message);
        }
    }
}
