namespace PlataformaEducacional.Core.Messages.Base
{
    public abstract class Message
    {
        [System.Text.Json.Serialization.JsonInclude]
        public Guid Id { get; private init; }

        [System.Text.Json.Serialization.JsonInclude]
        public string MessageType { get; private init; }

        [System.Text.Json.Serialization.JsonInclude]
        public Guid AggregateId { get; protected set; }

        [System.Text.Json.Serialization.JsonInclude]
        public DateTime CreatedAtUtc { get; private init; }

        protected Message()
        {
            Id = Guid.NewGuid();
            MessageType = GetType().Name;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public override string ToString()
            => $"{MessageType} [Id={Id}, AggregateId={AggregateId}, CreatedAt={CreatedAtUtc:O}]";

        public override bool Equals(object? obj) => obj is Message m && Id.Equals(m.Id);
        public override int GetHashCode() => Id.GetHashCode();
    }
}
