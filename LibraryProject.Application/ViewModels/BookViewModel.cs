using Core.Entities;

namespace Application.Models;

public class BookViewModel
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string ISBN { get; private set; }
    public int PublicationYear { get; private set; }
    public bool IsAvailable { get; set; } = true;

    public BookViewModel(Guid id, string title, string author, string isbn, int publicationYear, bool isAvailable)
    {
        Id = id;
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
        IsAvailable = isAvailable;
    }
    
    public BookViewModel() {}
}