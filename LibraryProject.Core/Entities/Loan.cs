namespace Core.Entities;

public class Loan : Entity
{
    public Guid CustomerId { get; set; }
    public User Customer { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
    public DateTime LoanDate { get; set; }
    
    public Loan() {}
    
    public Loan(Guid customerId, User customer, Guid bookId, Book book, DateTime loanDate)
    {
        CustomerId = customerId;
        Customer = customer;
        BookId = bookId;
        Book = book;
        LoanDate = loanDate;
    }
}