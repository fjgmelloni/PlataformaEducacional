using System;
using System.Collections.Generic;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.Core.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        private readonly List<Event> _domainEvents = new();
        public IReadOnlyCollection<Event> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(Event @event) => _domainEvents.Add(@event);
        protected void RemoveDomainEvent(Event @event) => _domainEvents.Remove(@event);
        public void ClearDomainEvents() => _domainEvents.Clear();

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not Entity other) return false;
            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity? a, Entity? b) => Equals(a, b);
        public static bool operator !=(Entity? a, Entity? b) => !(a == b);

        public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

        public override string ToString() => $"{GetType().Name} [Id={Id}]";

        public virtual bool IsValid() => throw new NotImplementedException();
    }
}
