using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.Core.Tests
{
    public class EntityTests
    {
        [Fact(DisplayName = nameof(Entity_ShouldCreateNewId_OnCreation))]
        [Trait("Category", "Core - Entity")]
        public void Entity_ShouldCreateNewId_OnCreation()
        {
            var entity = new TestEntity();

            Assert.NotEqual(Guid.Empty, entity.Id);
        }

        [Fact(DisplayName = nameof(AddDomainEvent_ShouldAddEventToCollection))]
        [Trait("Category", "Core - Entity")]
        public void AddDomainEvent_ShouldAddEventToCollection()
        {
            var entity = new TestEntity();
            var domainEvent = new FakeDomainEvent();

            entity.ExposeAddDomainEvent(domainEvent);

            Assert.Single(entity.DomainEvents);
            Assert.Contains(domainEvent, entity.DomainEvents);
        }

        [Fact(DisplayName = nameof(RemoveDomainEvent_ShouldRemoveEventFromCollection))]
        [Trait("Category", "Core - Entity")]
        public void RemoveDomainEvent_ShouldRemoveEventFromCollection()
        {
            var entity = new TestEntity();
            var domainEvent = new FakeDomainEvent();

            entity.ExposeAddDomainEvent(domainEvent);
            Assert.Single(entity.DomainEvents);

            entity.ExposeRemoveDomainEvent(domainEvent);

            Assert.Empty(entity.DomainEvents);
        }

        [Fact(DisplayName = nameof(ClearDomainEvents_ShouldRemoveAllEvents))]
        [Trait("Category", "Core - Entity")]
        public void ClearDomainEvents_ShouldRemoveAllEvents()
        {
            var entity = new TestEntity();

            entity.ExposeAddDomainEvent(new FakeDomainEvent());
            entity.ExposeAddDomainEvent(new FakeDomainEvent());
            Assert.Equal(2, entity.DomainEvents.Count);

            entity.ClearDomainEvents();

            Assert.Empty(entity.DomainEvents);
        }

        [Fact(DisplayName = nameof(Equals_ShouldReturnTrue_WhenIdsAreEqual))]
        [Trait("Category", "Core - Entity")]
        public void Equals_ShouldReturnTrue_WhenIdsAreEqual()
        {
            var id = Guid.NewGuid();
            var a = new TestEntity { Id = id };
            var b = new TestEntity { Id = id };

            Assert.True(a.Equals(b));
        }

        [Fact(DisplayName = nameof(Equals_ShouldReturnFalse_WhenIdsAreDifferent))]
        [Trait("Category", "Core - Entity")]
        public void Equals_ShouldReturnFalse_WhenIdsAreDifferent()
        {
            var a = new TestEntity();
            var b = new TestEntity();

            Assert.False(a.Equals(b));
        }

        [Fact(DisplayName = nameof(EqualityOperator_ShouldReturnTrue_WhenBothAreNull))]
        [Trait("Category", "Core - Entity")]
        public void EqualityOperator_ShouldReturnTrue_WhenBothAreNull()
        {
            TestEntity? a = null;
            TestEntity? b = null;

            Assert.True(a == b);
        }

        [Fact(DisplayName = nameof(EqualityOperator_ShouldReturnTrue_WhenIdsAreEqual))]
        [Trait("Category", "Core - Entity")]
        public void EqualityOperator_ShouldReturnTrue_WhenIdsAreEqual()
        {
            var id = Guid.NewGuid();
            var a = new TestEntity { Id = id };
            var b = new TestEntity { Id = id };

            Assert.True(a == b);
        }

        [Fact(DisplayName = nameof(EqualityOperator_ShouldReturnFalse_WhenIdsAreDifferent))]
        [Trait("Category", "Core - Entity")]
        public void EqualityOperator_ShouldReturnFalse_WhenIdsAreDifferent()
        {
            var a = new TestEntity();
            var b = new TestEntity();

            Assert.False(a == b);
        }
    }

    public sealed class FakeDomainEvent : Event
    {
        public FakeDomainEvent() { }
    }

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

        public override bool IsValid() => true;
    }
}
