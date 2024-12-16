namespace Core.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string ISBN { get; private set; }
    public int PublicationYear { get; private set; }
    public List<Loan> Loans { get; private set; }
    
    public Book() {}
}