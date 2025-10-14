using MediatR;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacional.Core.Messages.Common.DomainEvents;

namespace PlataformaEducacao.Core.Communication.Mediator
{
    public sealed class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        public MediatorHandler(IMediator mediator) => _mediator = mediator;
        public Task<bool> SendCommandAsync<T>(T command) where T : Command
            => _mediator.Send(command);
        public Task PublishEventAsync<T>(T @event) where T : Event
            => _mediator.Publish(@event);
        public Task PublishNotificationAsync<T>(T notification) where T : INotification
            => _mediator.Publish(notification);
        public Task PublishDomainEventAsync<T>(T domainEvent) where T : DomainEvent
            => _mediator.Publish(domainEvent);
    }
}
