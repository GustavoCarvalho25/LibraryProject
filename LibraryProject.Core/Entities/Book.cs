using Core.ValueObjects;

namespace Core.Entities;

public class Book : Entity
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public ISBN ISBN { get; private set; }
    public int PublicationYear { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public virtual List<Loan>? Loans { get; set; }
    
    public Book(string title, string author, string isbn, int publicationYear, bool isAvailable = true)
    {
        Title = title;
        Author = author;
        ISBN = new ISBN(isbn);
        PublicationYear = publicationYear;
        IsAvailable = isAvailable;
    }
    
    public Book(string title, string author, string isbn, int publicationYear)
    {
        Title = title;
        Author = author;
        ISBN = new ISBN(isbn);
        PublicationYear = publicationYear;
    }
    
    public Loan LoanTo(User user)
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Book is not available for loan");
            
        IsAvailable = false;
        var loan = new Loan(user.Id, user, Id, this, DateTime.Now);
        
        if (Loans == null)
            Loans = new List<Loan>();
            
        Loans.Add(loan);
        return loan;
    }
    
    public void Return()
    {
        if (IsAvailable)
            throw new InvalidOperationException("Book is already available");
            
        IsAvailable = true;
    }
}