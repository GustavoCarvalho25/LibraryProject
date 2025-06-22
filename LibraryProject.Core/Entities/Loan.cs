namespace Core.Entities;

public class Loan : Entity
{
    public Guid CustomerId { get; set; }
    public virtual User? Customer { get; set; }
    public Guid BookId { get; set; }
    public virtual Book? Book { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    
    protected Loan() {}
    
    public Loan(Guid customerId, Guid bookId, DateTime loanDate)
    {
        CustomerId = customerId;
        BookId = bookId;
        LoanDate = loanDate;
    }
}