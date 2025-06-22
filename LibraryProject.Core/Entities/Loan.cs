using Core.Events;

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

    public void Return()
    {
        if (ReturnDate != null)
            throw new InvalidOperationException("Loan has already been returned");
            
        ReturnDate = DateTime.Now;
        
        // Calcular dias de atraso
        var dueDate = LoanDate.AddDays(14); // 14 dias para devolução
        var daysLate = ReturnDate > dueDate ? (int)(ReturnDate.Value - dueDate).TotalDays : 0;
        
        // Adicionar evento de domínio
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