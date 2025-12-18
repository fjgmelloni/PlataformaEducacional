using Moq;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.Core.Tests
{
    public class EntityTests
    {
        private readonly Mock<Event> _eventMock;

        public EntityTests()
        {
            _eventMock = new Mock<Event>();
        }

        [Fact(DisplayName = nameof(Entity_ShouldCreateNewId_OnCreation))]
        [Trait("Category", "Core - Entity")]
        public void Entity_ShouldCreateNewId_OnCreation()
        {
            // Arrange & Act
            var entity = new TestEntity();

            // Assert
            Assert.NotEqual(Guid.Empty, entity.Id);
        }

        [Fact(DisplayName = nameof(AddDomainEvent_ShouldAddEventToCollection))]
        [Trait("Category", "Core - Entity")]
        public void AddDomainEvent_ShouldAddEventToCollection()
        {
            // Arrange
            var entity = new TestEntity();
            var domainEvent = _eventMock.Object;

            // Act
            entity.ExposeAddDomainEvent(domainEvent);

            // Assert
            Assert.Single(entity.DomainEvents);
            Assert.Contains(domainEvent, entity.DomainEvents);
        }

        [Fact(DisplayName = nameof(RemoveDomainEvent_ShouldRemoveEventFromCollection))]
        [Trait("Category", "Core - Entity")]
        public void RemoveDomainEvent_ShouldRemoveEventFromCollection()
        {
            // Arrange
            var entity = new TestEntity();
            var domainEvent = _eventMock.Object;

            entity.ExposeAddDomainEvent(domainEvent);
            Assert.Single(entity.DomainEvents);

            // Act
            entity.ExposeRemoveDomainEvent(domainEvent);

            // Assert
            Assert.Empty(entity.DomainEvents);
        }

        [Fact(DisplayName = nameof(ClearDomainEvents_ShouldRemoveAllEvents))]
        [Trait("Category", "Core - Entity")]
        public void ClearDomainEvents_ShouldRemoveAllEvents()
        {
            // Arrange
            var entity = new TestEntity();

            entity.ExposeAddDomainEvent(_eventMock.Object);
            entity.ExposeAddDomainEvent(_eventMock.Object);
            Assert.Equal(2, entity.DomainEvents.Count);

            // Act
            entity.ClearDomainEvents();

            // Assert
            Assert.Empty(entity.DomainEvents);
        }

        [Fact(DisplayName = nameof(Equals_ShouldReturnTrue_WhenIdsAreEqual))]
        [Trait("Category", "Core - Entity")]
        public void Equals_ShouldReturnTrue_WhenIdsAreEqual()
        {
            // Arrange
            var id = Guid.NewGuid();
            var a = new TestEntity { Id = id };
            var b = new TestEntity { Id = id };

            // Act & Assert
            Assert.True(a.Equals(b));
        }

        [Fact(DisplayName = nameof(Equals_ShouldReturnFalse_WhenIdsAreDifferent))]
        [Trait("Category", "Core - Entity")]
        public void Equals_ShouldReturnFalse_WhenIdsAreDifferent()
        {
            // Arrange
            var a = new TestEntity();
            var b = new TestEntity();

            // Act & Assert
            Assert.False(a.Equals(b));
        }

        [Fact(DisplayName = nameof(EqualityOperator_ShouldReturnTrue_WhenBothAreNull))]
        [Trait("Category", "Core - Entity")]
        public void EqualityOperator_ShouldReturnTrue_WhenBothAreNull()
        {
            // Arrange
            TestEntity? a = null;
            TestEntity? b = null;

            // Act & Assert
            Assert.True(a == b);
        }

        [Fact(DisplayName = nameof(EqualityOperator_ShouldReturnTrue_WhenIdsAreEqual))]
        [Trait("Category", "Core - Entity")]
        public void EqualityOperator_ShouldReturnTrue_WhenIdsAreEqual()
        {
            // Arrange
            var id = Guid.NewGuid();
            var a = new TestEntity { Id = id };
            var b = new TestEntity { Id = id };

            // Act & Assert
            Assert.True(a == b);
        }

        [Fact(DisplayName = nameof(EqualityOperator_ShouldReturnFalse_WhenIdsAreDifferent))]
        [Trait("Category", "Core - Entity")]
        public void EqualityOperator_ShouldReturnFalse_WhenIdsAreDifferent()
        {
            // Arrange
            var a = new TestEntity();
            var b = new TestEntity();

            // Act & Assert
            Assert.False(a == b);
        }
    }

    // 👇 entidade fake só para expor métodos protegidos
    public class TestEntity : Entity
    {
        public void ExposeAddDomainEvent(Event @event) => AddDomainEvent(@event);
        public void ExposeRemoveDomainEvent(Event @event) => RemoveDomainEvent(@event);
    }
}
