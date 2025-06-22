using Application.Models;
using Application.Queries.Books;
using AutoMapper;
using Core.Repository;
using Core.Shared;
using MediatR;

namespace Application.Handlers.Book;

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
        
        if (book == null)
        {
            return ResultViewModel<BookViewModel>.Error($"Book with ID {request.Id} not found.");
        }
        
        var bookViewModel = _mapper.Map<BookViewModel>(book);
        return ResultViewModel<BookViewModel>.Success(bookViewModel);
    }

    public async Task<ResultViewModel<PagedResult<BookViewModel>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Usar o método paginado do repositório
            var pagedBooks = await _bookRepository.GetPagedAsync(
                request.Options,
                null, // sem filtro adicional
                cancellationToken);
            
            // Mapear os resultados
            var bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(pagedBooks.Items);
            
            // Criar o resultado paginado com os ViewModels
            var pagedResult = new PagedResult<BookViewModel>(
                bookViewModels,
                pagedBooks.TotalCount,
                pagedBooks.PageNumber,
                pagedBooks.PageSize);
            
            return ResultViewModel<PagedResult<BookViewModel>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return ResultViewModel<PagedResult<BookViewModel>>.Error(ex.Message);
        }
    }

    public async Task<ResultViewModel<PagedResult<BookViewModel>>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Usar o método paginado com filtro por autor
            var pagedBooks = await _bookRepository.GetPagedAsync(
                request.Options,
                book => book.Author.Contains(request.AuthorName),
                cancellationToken);
            
            // Mapear os resultados
            var bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(pagedBooks.Items);
            
            // Criar o resultado paginado com os ViewModels
            var pagedResult = new PagedResult<BookViewModel>(
                bookViewModels,
                pagedBooks.TotalCount,
                pagedBooks.PageNumber,
                pagedBooks.PageSize);
            
            return ResultViewModel<PagedResult<BookViewModel>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return ResultViewModel<PagedResult<BookViewModel>>.Error(ex.Message);
        }
    }
}
