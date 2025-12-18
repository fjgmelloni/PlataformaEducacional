using MediatR;
using System.Collections.Concurrent;

namespace PlataformaEducacional.Core.Messages.CommonMessages.Notifications
{
    public sealed class DomainNotificationHandler :
        INotificationHandler<DomainNotification>,
        IDisposable
    {
        private readonly ConcurrentQueue<DomainNotification> _notifications = new();

        public Task Handle(
            DomainNotification notification,
            CancellationToken cancellationToken)
        {
            _notifications.Enqueue(notification);
            return Task.CompletedTask;
        }

        public IReadOnlyCollection<DomainNotification> GetNotifications()
            => _notifications.ToArray();

        public bool HasNotifications()
            => !_notifications.IsEmpty;

        public void Dispose()
        {
            Clear();
        }

        private void Clear()
        {
            while (_notifications.TryDequeue(out _)) { }
        }
    }
}
