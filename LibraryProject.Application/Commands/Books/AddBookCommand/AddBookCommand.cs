using Application.Models;
using MediatR;

namespace Application.Commands.Books.AddBookCommand;

public class AddBookCommand : IRequest<ResultViewModel<BookViewModel>>
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    
    public AddBookCommand(string title, string author, string isbn, int publicationYear)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
    }
}