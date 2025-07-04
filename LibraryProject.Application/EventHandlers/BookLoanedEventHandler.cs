using Core.Events;
using Core.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers;

public class BookLoanedEventHandler : INotificationHandler<BookLoanedEvent>
{
    private readonly ILogger<BookLoanedEventHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    
    public BookLoanedEventHandler(
        ILogger<BookLoanedEventHandler> logger,
        IUserRepository userRepository,
        IBookRepository bookRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _bookRepository = bookRepository;
    }
    
    public async Task Handle(BookLoanedEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(notification.Loan.CustomerId);
        var book = await _bookRepository.GetById(notification.Loan.BookId);
        
        if (user is not null && book is not null)
        {
            _logger.LogInformation(
                "Book '{Title}' loaned to '{UserName}' in {LoanDate}. " +
                "Devolution date: {DueDate}",
                book.Title,
                user.Name,
                notification.Loan.LoanDate.ToString("dd/MM/yyyy"),
                notification.Loan.DueDate.ToString("dd/MM/yyyy")
            );
            
            // Exemplo: await _emailService.SendLoanConfirmationEmailAsync(user.Email, book.Title, notification.DueDate);
        }
    }
}
