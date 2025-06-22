using Application.Models;
using Application.Queries.Books;
using AutoMapper;
using Core.Repository;
using MediatR;

namespace Application.Handlers;

public class BookQueryHandler : 
    IRequestHandler<GetBookByIdQuery, ResultViewModel<BookViewModel>>,
    IRequestHandler<GetAllBooksQuery, ResultViewModel<IEnumerable<BookViewModel>>>,
    IRequestHandler<GetBooksByAuthorQuery, ResultViewModel<IEnumerable<BookViewModel>>>
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
        
        if (book == null)
        {
            return ResultViewModel<BookViewModel>.Error($"Book with ID {request.Id} not found.");
        }
        
        var bookViewModel = _mapper.Map<BookViewModel>(book);
        return ResultViewModel<BookViewModel>.Success(bookViewModel);
    }

    public async Task<ResultViewModel<IEnumerable<BookViewModel>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAll();
        
        if (books == null || !books.Any())
        {
            return ResultViewModel<IEnumerable<BookViewModel>>.Success(new List<BookViewModel>());
        }
        
        var bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(books);
        return ResultViewModel<IEnumerable<BookViewModel>>.Success(bookViewModels);
    }

    public async Task<ResultViewModel<IEnumerable<BookViewModel>>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetBooksByAuthor(request.AuthorName);
        
        if (books == null || !books.Any())
        {
            return ResultViewModel<IEnumerable<BookViewModel>>.Success(new List<BookViewModel>());
        }
        
        var bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(books);
        return ResultViewModel<IEnumerable<BookViewModel>>.Success(bookViewModels);
    }
}
