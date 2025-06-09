namespace Core.Entities;

public class Book : Entity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public List<Loan> Loans { get; set; }
    
    public Book(string title, string author, string isbn, int publicationYear, List<Loan> loans) : base()
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
        Loans = loans;
    }
    
    public Book() {}
}