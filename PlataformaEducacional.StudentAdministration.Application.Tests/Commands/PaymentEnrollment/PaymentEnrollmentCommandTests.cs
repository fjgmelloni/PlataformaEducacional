using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PaymentEnrollment;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.PaymentEnrollment
{
    public class PaymentEnrollmentCommandTests
    {
        private PaymentEnrollmentCommand CreateValidCommand()
        {
            return new PaymentEnrollmentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                500,
                "John Doe",
                "49927398716",
                "12/26",
                "123"
            );
        }

        [Fact(DisplayName = "Constructor should assign all properties correctly")]
        public void Constructor_ShouldAssignProperties()
        {
            var enrollmentId = Guid.NewGuid();
            var studentId = Guid.NewGuid();

            var command = new PaymentEnrollmentCommand(enrollmentId, studentId, 500, "John Doe", "49927398716", "12/26", "123");

            Assert.Equal(enrollmentId, command.EnrollmentId);
            Assert.Equal(studentId, command.StudentId);
            Assert.Equal(500, command.Total);
            Assert.Equal("John Doe", command.CardName);
        }

        [Fact(DisplayName = "Valid command should return true")]
        public void ValidCommand_ShouldReturnTrue()
        {
            var command = CreateValidCommand();
            Assert.True(command.IsValid());
        }

        [Fact(DisplayName = "Invalid card number should produce error")]
        public void InvalidCardNumber_ShouldAddError()
        {
            var command = new PaymentEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid(), 500, "John Doe", "123", "12/26", "123");
            command.IsValid();
            Assert.False(command.ValidationResult.IsValid);
        }
    }
}
