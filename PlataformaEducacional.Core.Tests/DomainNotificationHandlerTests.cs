using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Core.Tests
{
    public class DomainNotificationHandlerTests
    {
        private readonly DomainNotificationHandler _handler;

        public DomainNotificationHandlerTests()
        {
            _handler = new DomainNotificationHandler();
        }

        [Fact(DisplayName = nameof(Handle_ShouldAddNotificationToList))]
        [Trait("Category", "Content Management - Core - DomainNotificationHandler")]
        public async Task Handle_ShouldAddNotificationToList()
        {
            // Arrange
            var notification = new DomainNotification("key1", "value1");

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            Assert.True(_handler.HasNotifications());
            Assert.Single(_handler.GetNotifications());
            Assert.Equal(notification, _handler.GetNotifications().First());
        }

        [Fact(DisplayName = nameof(Handle_ShouldAddMultipleNotificationsToList))]
        [Trait("Category", "Content Management - Core - DomainNotificationHandler")]
        public async Task Handle_ShouldAddMultipleNotificationsToList()
        {
            // Arrange
            var notification1 = new DomainNotification("key1", "value1");
            var notification2 = new DomainNotification("key2", "value2");

            // Act
            await _handler.Handle(notification1, CancellationToken.None);
            await _handler.Handle(notification2, CancellationToken.None);

            // Assert
            Assert.True(_handler.HasNotifications());
            Assert.Equal(2, _handler.GetNotifications().Count());
        }

        [Fact(DisplayName = nameof(HasNotifications_ShouldReturnFalse_WhenNoNotificationsExist))]
        [Trait("Category", "Content Management - Core - DomainNotificationHandler")]
        public void HasNotifications_ShouldReturnFalse_WhenNoNotificationsExist()
        {
            // Arrange & Act
            var hasNotifications = _handler.HasNotifications();

            // Assert
            Assert.False(hasNotifications);
        }

        [Fact(DisplayName = nameof(Dispose_ShouldClearNotifications))]
        [Trait("Category", "Content Management - Core - DomainNotificationHandler")]
        public async Task Dispose_ShouldClearNotifications()
        {
            // Arrange
            var notification = new DomainNotification("key1", "value1");
            await _handler.Handle(notification, CancellationToken.None);

            Assert.True(_handler.HasNotifications());

            // Act
            _handler.Dispose();

            // Assert
            Assert.False(_handler.HasNotifications());
            Assert.Empty(_handler.GetNotifications());
        }
    }
}
