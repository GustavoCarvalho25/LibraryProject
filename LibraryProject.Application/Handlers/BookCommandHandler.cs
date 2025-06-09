using Application.Commands.Books;
using Application.Models;
using Core.Entities;
using Core.Repository;
using MediatR;

namespace Application.Handlers;

public class BookCommandHandler : IRequestHandler<AddBookCommand, ResultViewModel<BookViewModel>>
{
    private readonly IBookRepository _bookRepository;

    public BookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<ResultViewModel<BookViewModel>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            PublicationYear = request.PublicationYear
        };

        var result = await _bookRepository.Add(book);
        
        if (result is null)
        {
            return ResultViewModel<BookViewModel>.Error("Failed to add book.");
        }

        return ResultViewModel<BookViewModel>.Success(BookViewModel.ToViewModel(book));
    }
}