using Core.Events;

namespace Core.Entities;

public class Loan : Entity
{
    public Guid CustomerId { get; set; }
    public virtual User? Customer { get; set; }
    public Guid BookId { get; set; }
    public virtual Book? Book { get; set; }
    public DateTime LoanDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    
    protected Loan() {}
    
    public Loan(Guid customerId, Guid bookId, DateTime loanDate)
    {
        CustomerId = customerId;
        BookId = bookId;
        LoanDate = loanDate;
    }
    
    public void SetDueDate(int loanPeriodInDays = 14)
    {
        DueDate = LoanDate.AddDays(loanPeriodInDays);
    }
    
    public void ExtendDueDate(int extraDays)
    {
        if (ReturnDate != null)
            throw new InvalidOperationException("Cannot extend due date of a returned loan");
            
        DueDate = DueDate.AddDays(extraDays);
    }

    public void Return()
    {
        if (ReturnDate != null)
            throw new InvalidOperationException("Loan has already been returned");
            
        ReturnDate = DateTime.Now;
        
        var daysLate = ReturnDate > DueDate ? (int)(ReturnDate.Value - DueDate).TotalDays : 0;
        
        AddDomainEvent(new BookReturnedEvent(BookId, CustomerId, Id, ReturnDate.Value, daysLate));
    }
    
    public bool IsOverdue(int loanPeriodInDays = 14)
    {
        if (ReturnDate != null)
            return false;
            
        return (DateTime.Now - LoanDate).TotalDays > loanPeriodInDays;
    }
    
    public int DaysOverdue(int loanPeriodInDays = 14)
    {
        if (!IsOverdue(loanPeriodInDays))
            return 0;
            
        return (int)(DateTime.Now - LoanDate.AddDays(loanPeriodInDays)).TotalDays;
    }
}