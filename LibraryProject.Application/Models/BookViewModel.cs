using Core.Entities;

namespace Application.Models;

public class BookViewModel
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string ISBN { get; private set; }
    public int PublicationYear { get; private set; }

    public BookViewModel(Guid id, string title, string author, string isbn, int publicationYear)
    {
        Id = id;
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
    }
    
    public static BookViewModel ToViewModel(Book book)
    => new BookViewModel(book.Id, book.Title, book.Author, book.ISBN, book.PublicationYear);
}