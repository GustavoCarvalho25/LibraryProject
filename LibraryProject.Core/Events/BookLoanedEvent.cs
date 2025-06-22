namespace Core.Events;

public class BookLoanedEvent : DomainEvent
{
    public Guid BookId { get; }
    public Guid UserId { get; }
    public DateTime LoanDate { get; }
    public DateTime DueDate { get; }
    
    public BookLoanedEvent(Guid bookId, Guid userId, DateTime loanDate, DateTime dueDate)
    {
        BookId = bookId;
        UserId = userId;
        LoanDate = loanDate;
        DueDate = dueDate;
    }
}
