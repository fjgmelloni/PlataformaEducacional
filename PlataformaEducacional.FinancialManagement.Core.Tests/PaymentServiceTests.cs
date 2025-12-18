using Moq;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain.DTO;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.FinancialManagement.Core;

namespace PlataformaEducacional.FinancialManagement.Core.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<ICreditCardPaymentFacade> _creditCardFacadeMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IMediatorHandler> _mediatorMock;
        private readonly PaymentService _service;

        private readonly EnrollmentPayment _validEnrollmentPayment;

        public PaymentServiceTests()
        {
            _creditCardFacadeMock = new Mock<ICreditCardPaymentFacade>();
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _mediatorMock = new Mock<IMediatorHandler>();

            _service = new PaymentService(
                _creditCardFacadeMock.Object,
                _paymentRepositoryMock.Object,
                _mediatorMock.Object
            );

            _validEnrollmentPayment = new EnrollmentPayment(
          EnrollmentId: Guid.NewGuid(),
          StudentId: Guid.NewGuid(),
          CourseId: Guid.NewGuid(),
          Amount: 500,
          CardholderName: "John Doe",
          CardNumber: "1234567890123456",
          CardExpiration: "06/26",
          CardCvv: "123"
      );

        }

        [Fact(DisplayName = "Should persist payment and publish success event when transaction is paid")]
        [Trait("Category", "Financial Management - PaymentService")]
        public async Task ProcessEnrollmentPayment_ShouldPublishCompletedEvent_WhenTransactionIsPaid()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), 500);
            transaction.ChangeStatus(TransactionStatus.Paid);

            _creditCardFacadeMock
                .Setup(f => f.Charge(It.IsAny<Guid>(), It.IsAny<Payment>()))
                .Returns(transaction);

            // Act
            var result = await _service.ProcessEnrollmentPayment(_validEnrollmentPayment);

            // Assert
            Assert.Equal(TransactionStatus.Paid, result.Status);

            _paymentRepositoryMock.Verify(r => r.Add(It.IsAny<Payment>()), Times.Once);
            _paymentRepositoryMock.Verify(r => r.AddTransaction(transaction), Times.Once);

            _mediatorMock.Verify(
                m => m.PublishEventAsync(It.IsAny<EnrollmentPaymentCompletedEvent>()),
                Times.Once
            );

            _mediatorMock.Verify(
                m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()),
                Times.Never
            );
        }

        [Fact(DisplayName = "Should publish notification and rejection event when transaction is declined")]
        [Trait("Category", "Financial Management - PaymentService")]
        public async Task ProcessEnrollmentPayment_ShouldPublishRejectedEvent_WhenTransactionIsDeclined()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), 500);
            transaction.ChangeStatus(TransactionStatus.Declined);

            _creditCardFacadeMock
                .Setup(f => f.Charge(It.IsAny<Guid>(), It.IsAny<Payment>()))
                .Returns(transaction);

            // Act
            var result = await _service.ProcessEnrollmentPayment(_validEnrollmentPayment);

            // Assert
            Assert.Equal(TransactionStatus.Declined, result.Status);

            _paymentRepositoryMock.Verify(r => r.Add(It.IsAny<Payment>()), Times.Once);
            _paymentRepositoryMock.Verify(r => r.AddTransaction(transaction), Times.Once);

            _mediatorMock.Verify(
                m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()),
                Times.Once
            );

            _mediatorMock.Verify(
                m => m.PublishEventAsync(It.IsAny<EnrollmentPaymentRejectedEvent>()),
                Times.Once
            );
        }
    }
}
