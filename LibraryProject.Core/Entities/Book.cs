namespace Core.Entities;

public class Book : Entity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Isbn { get; set; }
    public int PublicationYear { get; set; }
    public virtual List<Loan> Loans { get; set; }
    
    protected Book() {}
    
    public Book(string title, string author, string isbn, int publicationYear) : base()
    {
        Title = title;
        Author = author;
        Isbn = isbn;
        PublicationYear = publicationYear;
    }

    public void Update(string? title, string? author, string? isbn, int? publicationYear)
    {
        Title = title ?? this.Title;
        Author = author ?? this.Author;
        Isbn = isbn ?? this.Isbn;
        PublicationYear = publicationYear ?? this.PublicationYear;
    }
}