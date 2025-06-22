using Core.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Events;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventPublisher> _logger;
    
    public DomainEventPublisher(IMediator mediator, ILogger<DomainEventPublisher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task PublishEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            _logger.LogInformation($"Publicando evento de domínio: {domainEvent.GetType().Name} ({domainEvent.Id})");
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
