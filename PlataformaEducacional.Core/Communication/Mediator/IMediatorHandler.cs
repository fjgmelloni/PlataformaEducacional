using MediatR;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacional.Core.Messages.Common.DomainEvents;

namespace PlataformaEducacao.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task<bool> SendCommandAsync<T>(T command) where T : Command;
        Task PublishEventAsync<T>(T @event) where T : Event;
        Task PublishNotificationAsync<T>(T notification) where T : INotification;
        Task PublishDomainEventAsync<T>(T domainEvent) where T : DomainEvent;
    }
}
