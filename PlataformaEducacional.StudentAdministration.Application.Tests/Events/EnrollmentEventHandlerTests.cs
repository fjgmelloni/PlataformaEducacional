using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.StudentAdministration.Application.Events;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Events;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Events
{
    public class EnrollmentEventHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly EnrollmentEventHandler _handler;

        public EnrollmentEventHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<EnrollmentEventHandler>();
        }

        [Fact(DisplayName = "Should send CompleteEnrollmentCommand when payment is completed")]
        [Trait("Category", "EnrollmentEventHandler")]
        public async Task Handle_PaymentCompleted_ShouldSend_CompleteEnrollmentCommand()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var evt = new EnrollmentPaymentCompletedEvent(enrollmentId, studentId, Guid.NewGuid(), Guid.NewGuid(), 100);

            // Act
            await _handler.Handle(evt, CancellationToken.None);

            // Assert
            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.SendCommandAsync(It.Is<CompleteEnrollmentCommand>(c =>
                    c.EnrollmentId == enrollmentId &&
                    c.StudentId == studentId)),
                Times.Once);
        }

        [Fact(DisplayName = "Should send CancelEnrollmentCommand when payment is rejected")]
        [Trait("Category", "EnrollmentEventHandler")]
        public async Task Handle_PaymentRejected_ShouldSend_CancelEnrollmentCommand()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var evt = new EnrollmentPaymentRejectedEvent(
                enrollmentId,
                studentId,
                Guid.NewGuid(),
                Guid.NewGuid(),
                100,
                "Payment rejected"); 


            // Act
            await _handler.Handle(evt, CancellationToken.None);

            // Assert
            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.SendCommandAsync(It.Is<CancelEnrollmentCommand>(c =>
                    c.EnrollmentId == enrollmentId &&
                    c.StudentId == studentId)),
                Times.Once);
        }

        [Fact(DisplayName = "Should send GenerateCertificateCommand when course is completed")]
        [Trait("Category", "EnrollmentEventHandler")]
        public async Task Handle_CourseCompleted_ShouldSend_GenerateCertificateCommand()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            var evt = new CourseCompletedEvent(enrollmentId);

            // Act
            await _handler.Handle(evt, CancellationToken.None);

            // Assert
            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.SendCommandAsync(It.Is<GenerateCertificateCommand>(c =>
                    c.EnrollmentId == enrollmentId)),
                Times.Once);
        }
    }
}
