using Core.Events;
using Core.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers;

public class BookReturnedEventHandler : INotificationHandler<BookReturnedEvent>
{
    private readonly ILogger<BookReturnedEventHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    
    public BookReturnedEventHandler(
        ILogger<BookReturnedEventHandler> logger,
        IUserRepository userRepository,
        IBookRepository bookRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _bookRepository = bookRepository;
    }
    
    public async Task Handle(BookReturnedEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(notification.UserId);
        var book = await _bookRepository.GetById(notification.BookId);
        
        if (user != null && book != null)
        {
            _logger.LogInformation(
                "Livro '{Title}' devolvido por '{UserName}' em {ReturnDate}. " +
                "Dias de atraso: {DaysLate}",
                book.Title,
                user.Name,
                notification.ReturnDate.ToString("dd/MM/yyyy"),
                notification.DaysLate
            );
            
            if (notification.DaysLate > 0)
            {
                _logger.LogWarning(
                    "Usuário '{UserName}' devolveu o livro '{Title}' com {DaysLate} dias de atraso. " +
                    "Uma multa pode ser aplicada.",
                    user.Name,
                    book.Title,
                    notification.DaysLate
                );
                
                // Exemplo: await _fineService.CalculateAndApplyFineAsync(notification.UserId, notification.DaysLate);
            }
        }
    }
}
