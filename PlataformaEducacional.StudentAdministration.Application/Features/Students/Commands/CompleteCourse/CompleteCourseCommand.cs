using FluentValidation;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse
{
    public class CompleteCourseCommand : Command
    {
        public CompleteCourseCommand(Guid enrollmentId)
        {
            EnrollmentId = enrollmentId;
        }

        public Guid EnrollmentId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CompleteCourseCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CompleteCourseCommandValidation : AbstractValidator<CompleteCourseCommand>
    {
        public CompleteCourseCommandValidation()
        {
            RuleFor(c => c.EnrollmentId)
                .NotEqual(Guid.Empty)
                .WithMessage("Enrollment ID must be valid.");
        }
    }
}
