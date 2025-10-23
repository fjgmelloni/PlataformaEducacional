using MediatR;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.Common.DomainEvents;

namespace PlataformaEducacional.Core.Communication.Mediator
{
    public sealed class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> SendCommandAsync<T>(T command) where T : Command
            => await _mediator.Send(command);

        public async Task PublishEventAsync<T>(T @event) where T : Event
            => await _mediator.Publish(@event);

        public async Task PublishNotificationAsync<T>(T notification) where T : INotification
            => await _mediator.Publish(notification);

        public async Task PublishDomainEventAsync<T>(T domainEvent) where T : DomainEvent
            => await _mediator.Publish(domainEvent);
    }
}
