using Core.Events;
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
    
    protected Book() {}
    
    public Book(string title, string author, string isbn, int publicationYear) : base()
    {
        Title = title;
        Author = author;
        ISBN = new ISBN(isbn);
        PublicationYear = publicationYear;
    }
    
    public void Update(string title, string author, string isbn, int publicationYear)
    {
        Title = title;
        Author = author;
        ISBN = new ISBN(isbn);
        PublicationYear = publicationYear;
    }
    
    public Loan LoanTo(User user)
    {
        if (!IsAvailable)
            return default;
            
        IsAvailable = false;
        var loan = new Loan(user.Id, Id, DateTime.Now);
        
        loan.SetDueDate();
        
        if (Loans == null)
            Loans = new List<Loan>();
            
        Loans.Add(loan);
        
        AddDomainEvent(new BookLoanedEvent(loan));
        
        return loan;
    }
    
    public void Return()
    {
        if (IsAvailable)
            throw new InvalidOperationException("Book is already available");
            
        IsAvailable = true;
    }
}