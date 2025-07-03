using Application.Models;
using Application.Queries.Books;
using AutoMapper;
using Core.Repository;
using Core.Shared;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Application.Handlers.BookHandlers;

public class BookQueryHandler : 
    IRequestHandler<GetBookByIdQuery, ResultViewModel<BookViewModel>>,
    IRequestHandler<GetAllBooksQuery, ResultViewModel<PagedResult<BookViewModel>>>,
    IRequestHandler<GetBooksByAuthorQuery, ResultViewModel<PagedResult<BookViewModel>>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BookQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<ResultViewModel<BookViewModel>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.Id);
        
        if (book is null)
            return ResultViewModel<BookViewModel>.Error($"Book with ID {request.Id} not found.");
        
        var bookViewModel = _mapper.Map<BookViewModel>(book);
        return ResultViewModel<BookViewModel>.Success(bookViewModel);
    }

    public async Task<ResultViewModel<PagedResult<BookViewModel>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetPagedAsync(request.Options);

        if (books.Items.IsNullOrEmpty())
            return ResultViewModel<PagedResult<BookViewModel>>.Success("Does not have books registered yet.");
            
        var bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(books.Items);
            
        var pagedResult = PagedResult<BookViewModel>.From(bookViewModels, books);
            
        return ResultViewModel<PagedResult<BookViewModel>>.Success(pagedResult);
    }

    public async Task<ResultViewModel<PagedResult<BookViewModel>>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetPagedAsync(request.Options,
            book => book.Author.Contains(request.AuthorName));
        
        if (books.Items.IsNullOrEmpty())
            return ResultViewModel<PagedResult<BookViewModel>>.Error("Does not have books registered with this author.");
            
        var bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(books.Items);
            
        var pagedResult = PagedResult<BookViewModel>.From(bookViewModels, books);
            
        return ResultViewModel<PagedResult<BookViewModel>>.Success(pagedResult);
    }
}
