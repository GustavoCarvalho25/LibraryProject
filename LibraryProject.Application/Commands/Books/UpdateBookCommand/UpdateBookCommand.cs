using Application.Models;
using MediatR;

namespace Application.Commands.Books;

public class UpdateBookCommand : IRequest<ResultViewModel<BookViewModel>>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    
    public UpdateBookCommand(Guid id, string title, string author, string isbn, int publicationYear)
    {
        Id = id;
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
    }
}
