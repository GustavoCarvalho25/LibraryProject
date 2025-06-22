using Core.Entities;

namespace Application.Models;

public class LoanViewModel
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public Guid BookId { get; private set; }
    public string BookTitle { get; private set; }
    public DateTime LoanDate { get; private set; }
    
    public LoanViewModel(Guid id, Guid customerId, string customerName, Guid bookId, string bookTitle, DateTime loanDate)
    {
        Id = id;
        CustomerId = customerId;
        CustomerName = customerName;
        BookId = bookId;
        BookTitle = bookTitle;
        LoanDate = loanDate;
    }
}
