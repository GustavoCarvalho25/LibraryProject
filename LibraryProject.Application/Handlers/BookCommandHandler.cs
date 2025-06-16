using Application.Commands.Books;
using Application.Commands.Books.DeleteBookCommand;
using Application.Commands.Books.UpdateBookCommand;
using Application.Models;
using Core.Entities;
using Core.Repository;
using MediatR;

namespace Application.Handlers;

public class BookCommandHandler
    : IRequestHandler<AddBookCommand, ResultViewModel<BookViewModel>>,
    IRequestHandler<UpdateBookCommand, ResultViewModel<BookViewModel>>,
    IRequestHandler<DeleteBookCommand,ResultViewModel<BookViewModel>>
{
    private readonly IBookRepository _bookRepository;

    public BookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<ResultViewModel<BookViewModel>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        (
            request.Title,
            request.Author,
            request.ISBN,
            request.PublicationYear
        );

        var result = await _bookRepository.Add(book);
        
        if (result is null)
        {
            return ResultViewModel<BookViewModel>.Error("Failed to add book.");
        }

        return ResultViewModel<BookViewModel>.Success(BookViewModel.ToViewModel(book));
    }

    public async Task<ResultViewModel<BookViewModel>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.Id);
        
        if (book is null)
        {
            return await Task.FromResult(ResultViewModel<BookViewModel>.Error("Book not found."));
        }
        
        book.Update(request.Title, request.Author, request.Isbn, request.PublicationYear);
        
        var result = await _bookRepository.Update(book);
        
        if (result is null)
        {
            return await Task.FromResult(ResultViewModel<BookViewModel>.Error("Failed to update book."));
        }
        
        return await Task.FromResult(ResultViewModel<BookViewModel>.Success(BookViewModel.ToViewModel(book)));
    }

    public Task<ResultViewModel<BookViewModel>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}