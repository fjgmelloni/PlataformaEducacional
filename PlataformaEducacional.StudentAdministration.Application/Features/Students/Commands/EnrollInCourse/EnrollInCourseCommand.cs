using FluentValidation;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse
{
    public sealed class EnrollInCourseCommand : Command
    {
        public EnrollInCourseCommand(Guid studentId, Guid courseId, string courseName, int totalLessons, decimal value)
        {
            StudentId = studentId;
            CourseId = courseId;
            CourseName = courseName;
            TotalLessons = totalLessons;
            Value = value;
        }

        public Guid StudentId { get; }
        public Guid CourseId { get; }
        public string CourseName { get; }
        public int TotalLessons { get; }
        public decimal Value { get; }

        public override bool IsValid()
        {
            var result = new EnrollInCourseCommandValidator().Validate(this);
            ValidationResult.Errors.Clear();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ValidationResult.AddError(error.ErrorMessage);
            }
            return ValidationResult.IsValid;
        }

    }

    public sealed class EnrollInCourseCommandValidator : AbstractValidator<EnrollInCourseCommand>
    {
        public EnrollInCourseCommandValidator()
        {
            RuleFor(c => c.StudentId)
                .NotEmpty().WithMessage("O ID do aluno é obrigatório.");

            RuleFor(c => c.CourseId)
                .NotEmpty().WithMessage("O ID do curso é obrigatório.");

            RuleFor(c => c.CourseName)
                .NotEmpty().WithMessage("O nome do curso é obrigatório.");

            RuleFor(c => c.TotalLessons)
                .GreaterThan(0).WithMessage("O curso deve ter pelo menos uma aula.");

            RuleFor(c => c.Value)
                .GreaterThan(0).WithMessage("O valor do curso deve ser maior que zero.");
        }
    }
}
