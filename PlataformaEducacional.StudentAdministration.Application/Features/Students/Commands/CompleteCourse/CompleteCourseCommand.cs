using FluentValidation;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PerformLesson;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse
{
    public sealed class CompleteCourseCommand : Command
    {
        public CompleteCourseCommand(Guid enrollmentId, Guid studentId)
        {
            EnrollmentId = enrollmentId;
            StudentId = studentId;
        }

        public Guid EnrollmentId { get; }
        public Guid StudentId { get; }

        public override bool IsValid()
        {
            var result = new CompleteCourseCommandValidator().Validate(this);
            ValidationResult.Errors.Clear();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ValidationResult.AddError(error.ErrorMessage);
            }
            return ValidationResult.IsValid;
        }
    }

    public sealed class CompleteCourseCommandValidator : AbstractValidator<CompleteCourseCommand>
    {
        public CompleteCourseCommandValidator()
        {
            RuleFor(c => c.StudentId)
                .NotEmpty()
                .WithMessage("O ID do aluno é obrigatório.");

            RuleFor(c => c.EnrollmentId)
                .NotEmpty()
                .WithMessage("O ID da matrícula é obrigatório.");
        }
    }
}
