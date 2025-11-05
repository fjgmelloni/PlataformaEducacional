using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PaymentEnrollment;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.PaymentEnrollment
{
    public class PaymentEnrollmentCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly PaymentEnrollmentCommandHandler _handler;

        public PaymentEnrollmentCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<PaymentEnrollmentCommandHandler>();
        }

        [Fact(DisplayName = "Should start payment and commit when enrollment exists")]
        public async Task ValidCommand_ShouldStartPaymentAndCommit()
        {
            var command = new PaymentEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid(), 500, "John Doe", "49927398716", "12/26", "123");
            var enrollment = new Enrollment(Guid.NewGuid(), "Test Course", 10, 500);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithStudentById(command.EnrollmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(enrollment);

            _mocker.GetMock<IStudentRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(EnrollmentStatus.ProcessingPayment, enrollment.EnrollmentStatus);
        }

        [Fact(DisplayName = "Should notify if enrollment is not found")]
        public async Task EnrollmentNotFound_ShouldNotify()
        {
            var command = new PaymentEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid(), 500, "John Doe", "49927398716", "12/26", "123");

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithStudentById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enrollment)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.Once);
        }
    }
}
