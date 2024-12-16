namespace Core.Entities;

public class Loan
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public User Customer { get; private set; }
    public Guid BookId { get; private set; }
    public Book Book { get; private set; }
    public DateTime LoanDate { get; private set; }
    
    public Loan() {}
}