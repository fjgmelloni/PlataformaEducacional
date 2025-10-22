using FluentValidation;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment
{
    public class CancelEnrollmentCommand : Command
    {
        public CancelEnrollmentCommand(Guid enrollmentId, Guid studentId)
        {
            EnrollmentId = enrollmentId;
            StudentId = studentId;
        }

        public Guid EnrollmentId { get; private set; }
        public Guid StudentId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CancelEnrollmentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CancelEnrollmentCommandValidation : AbstractValidator<CancelEnrollmentCommand>
    {
        public CancelEnrollmentCommandValidation()
        {
            RuleFor(c => c.StudentId)
                .NotEmpty()
                .WithMessage("Student ID is required.");

            RuleFor(c => c.EnrollmentId)
                .NotEmpty()
                .WithMessage("Enrollment ID is required.");
        }
    }
}
