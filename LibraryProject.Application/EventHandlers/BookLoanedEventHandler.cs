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
        var user = await _userRepository.GetById(notification.UserId);
        var book = await _bookRepository.GetById(notification.BookId);
        
        if (user != null && book != null)
        {
            _logger.LogInformation(
                "Livro '{Title}' emprestado para '{UserName}' em {LoanDate}. " +
                "Data de devolução prevista: {DueDate}",
                book.Title,
                user.Name,
                notification.LoanDate.ToString("dd/MM/yyyy"),
                notification.DueDate.ToString("dd/MM/yyyy")
            );
            
            // Exemplo: await _emailService.SendLoanConfirmationEmailAsync(user.Email, book.Title, notification.DueDate);
        }
    }
}
