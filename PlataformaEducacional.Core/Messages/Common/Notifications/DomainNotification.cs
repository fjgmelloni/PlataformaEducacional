using MediatR;
using PlataformaEducacao.Core.Messages.Base;

namespace PlataformaEducacao.Core.Messages.CommonMessages.Notifications
{
    public sealed class DomainNotification : Message, INotification
    {
        public string Key { get; }
        public string? Value { get; }
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
        public int Version { get; } = 1;

        public DomainNotification(string key, string? value = null)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
            => $"[DomainNotification] Key={Key}, Value={Value ?? "null"}, OccurredOn={OccurredOnUtc:O}";
    }
}
