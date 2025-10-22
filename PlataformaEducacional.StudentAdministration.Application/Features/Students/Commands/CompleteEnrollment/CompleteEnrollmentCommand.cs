using FluentValidation;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment
{
    public class CompleteEnrollmentCommand : Command
    {
        public CompleteEnrollmentCommand(Guid enrollmentId, Guid studentId)
        {
            EnrollmentId = enrollmentId;
            StudentId = studentId;
        }

        public Guid EnrollmentId { get; private set; }
        public Guid StudentId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CompleteEnrollmentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CompleteEnrollmentCommandValidation : AbstractValidator<CompleteEnrollmentCommand>
    {
        public CompleteEnrollmentCommandValidation()
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
