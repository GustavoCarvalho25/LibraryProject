using Core.Entities;

namespace Core.Events;

public class BookLoanedEvent : DomainEvent
{
    public Loan Loan;
    
    public BookLoanedEvent(Loan loan)
    {
        Loan = loan;
    }
}
