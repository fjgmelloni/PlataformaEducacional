using FluentValidation;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent
{
    public sealed class AddStudentCommand : Command
    {
        public AddStudentCommand(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public Guid UserId { get; }
        public string Name { get; }

        public override bool IsValid()
        {
            var result = new AddStudentCommandValidator().Validate(this);

            ValidationResult.Errors.Clear();

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ValidationResult.AddError(error.ErrorMessage);
                }
            }

            return ValidationResult.IsValid;
        }

    }

    public sealed class AddStudentCommandValidator : AbstractValidator<AddStudentCommand>
    {
        public AddStudentCommandValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Student name is required.");
        }
    }
}
