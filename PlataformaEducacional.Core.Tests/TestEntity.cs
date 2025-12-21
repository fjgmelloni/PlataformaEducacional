using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.Api.Tests.Core
{
    public sealed class TestEntity : Entity
    {
        public void ExposeAddDomainEvent(Event domainEvent)
        {
            AddDomainEvent(domainEvent);
        }

        public void ExposeRemoveDomainEvent(Event domainEvent)
        {
            RemoveDomainEvent(domainEvent);
        }

        public void ExposeClearDomainEvents()
        {
            ClearDomainEvents();
        }

        public override bool IsValid() => true;
    }
}
