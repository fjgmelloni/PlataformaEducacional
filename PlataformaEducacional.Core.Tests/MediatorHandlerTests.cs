using MediatR;
using Moq;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.Common.DomainEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Core.Tests
{
    public class MediatorHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly MediatorHandler _mediatorHandler;

        public MediatorHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mediatorHandler = new MediatorHandler(_mediatorMock.Object);
        }

        [Fact(DisplayName = nameof(SendCommandAsync_ShouldCallMediatorSend))]
        [Trait("Category", "Core - MediatorHandler")]
        public async Task SendCommandAsync_ShouldCallMediatorSend()
        {
            // Arrange
            var command = new TestCommand();

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _mediatorHandler.SendCommandAsync(command);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(command, It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.True(result);
        }

        [Fact(DisplayName = nameof(PublishEventAsync_ShouldCallMediatorPublish))]
        [Trait("Category", "Core - MediatorHandler")]
        public async Task PublishEventAsync_ShouldCallMediatorPublish()
        {
            // Arrange
            var @event = new TestEvent();

            // Act
            await _mediatorHandler.PublishEventAsync(@event);

            // Assert
            _mediatorMock.Verify(
                m => m.Publish(@event, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact(DisplayName = nameof(PublishNotificationAsync_ShouldCallMediatorPublish))]
        [Trait("Category", "Core - MediatorHandler")]
        public async Task PublishNotificationAsync_ShouldCallMediatorPublish()
        {
            // Arrange
            var notification = new DomainNotification("key", "value");

            // Act
            await _mediatorHandler.PublishNotificationAsync(notification);

            // Assert
            _mediatorMock.Verify(
                m => m.Publish(notification, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact(DisplayName = nameof(PublishDomainEventAsync_ShouldCallMediatorPublish))]
        [Trait("Category", "Core - MediatorHandler")]
        public async Task PublishDomainEventAsync_ShouldCallMediatorPublish()
        {
            // Arrange
            var domainEvent = new TestDomainEvent(Guid.NewGuid());

            // Act
            await _mediatorHandler.PublishDomainEventAsync(domainEvent);

            // Assert
            _mediatorMock.Verify(
                m => m.Publish(domainEvent, It.IsAny<CancellationToken>()),
                Times.Once);
        }


        private sealed class TestCommand : Command { }

        private sealed class TestEvent : Event { }

        private sealed class TestDomainEvent : DomainEvent
        {
            public TestDomainEvent(Guid aggregateId) : base(aggregateId)
            {
            }
        }
    }
}
