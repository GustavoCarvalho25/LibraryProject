namespace Core.Events;

public class BookReturnedEvent : DomainEvent
{
    public Guid BookId { get; }
    public Guid UserId { get; }
    public Guid LoanId { get; }
    public DateTime ReturnDate { get; }
    public int DaysLate { get; }
    
    public BookReturnedEvent(Guid bookId, Guid userId, Guid loanId, DateTime returnDate, int daysLate)
    {
        BookId = bookId;
        UserId = userId;
        LoanId = loanId;
        ReturnDate = returnDate;
        DaysLate = daysLate;
    }
}
