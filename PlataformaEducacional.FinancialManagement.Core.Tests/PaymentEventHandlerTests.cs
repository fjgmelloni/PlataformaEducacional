using Moq;
using PlataformaEducacional.Core.Domain.DTO;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.FinancialManagement.Core.Events;

namespace PlataformaEducacional.FinancialManagement.Core.Tests.Events
{
    public class PaymentEventHandlerTests
    {
        [Fact(DisplayName = "Handle should call payment service once when event is received")]
        [Trait("Category", "Financial Management - PaymentEventHandler")]
        public async Task Handle_WhenEventReceived_ShouldCallProcessEnrollmentPaymentOnce()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();

            paymentServiceMock
                .Setup(s => s.ProcessEnrollmentPayment(It.IsAny<EnrollmentPayment>()))
                .ReturnsAsync(new Transaction(Guid.NewGuid(), 250));

            var handler = new PaymentEventHandler(paymentServiceMock.Object);
           
            var evt = new PaymentStartedEvent(
                enrollmentId: Guid.NewGuid(),
                studentId: Guid.NewGuid(),
                courseId: Guid.NewGuid(),
                amount: 250,
                cardholderName: "Felicio",
                cardNumber: "11144445555666",
                cardExpiration: "12/26",
                cardCvv: "999",
                sourceContext: "Enrollment"
            );

            // Act
            await handler.Handle(evt, CancellationToken.None);

            // Assert
            paymentServiceMock.Verify(
                s => s.ProcessEnrollmentPayment(It.IsAny<EnrollmentPayment>()),
                Times.Once
            );
        }
    }
}
