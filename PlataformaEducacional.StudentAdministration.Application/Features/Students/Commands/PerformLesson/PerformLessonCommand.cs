using FluentValidation;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PerformLesson
{
    public sealed class PerformLessonCommand : Command
    {
        public PerformLessonCommand(Guid enrollmentId, Guid lessonId)
        {
            EnrollmentId = enrollmentId;
            LessonId = lessonId;
        }

        public Guid EnrollmentId { get; }
        public Guid LessonId { get; }

        public override bool IsValid()
        {
            var result = new PerformLessonCommandValidator().Validate(this);
            ValidationResult.Errors.Clear();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ValidationResult.AddError(error.ErrorMessage);
            }
            return ValidationResult.IsValid;
        }

    }

    public sealed class PerformLessonCommandValidator : AbstractValidator<PerformLessonCommand>
    {
        public PerformLessonCommandValidator()
        {
            RuleFor(c => c.EnrollmentId)
                .NotEmpty().WithMessage("O ID da matrícula é obrigatório.");

            RuleFor(c => c.LessonId)
                .NotEmpty().WithMessage("O ID da aula é obrigatório.");
        }
    }
}
