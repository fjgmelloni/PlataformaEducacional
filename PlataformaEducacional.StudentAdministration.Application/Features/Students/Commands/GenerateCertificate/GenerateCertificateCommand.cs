using FluentValidation;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate
{
    public sealed class GenerateCertificateCommand : Command
    {
        public GenerateCertificateCommand(Guid enrollmentId)
        {
            EnrollmentId = enrollmentId;
        }

        public Guid EnrollmentId { get; }

        public override bool IsValid()
        {
            var result = new GenerateCertificateCommandValidator().Validate(this);
            ValidationResult.Errors.Clear();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ValidationResult.AddError(error.ErrorMessage);
            }
            return ValidationResult.IsValid;
        }

    }

    public sealed class GenerateCertificateCommandValidator : AbstractValidator<GenerateCertificateCommand>
    {
        public GenerateCertificateCommandValidator()
        {
            RuleFor(c => c.EnrollmentId)
                .NotEmpty()
                .WithMessage("O ID da matrícula é obrigatório.");
        }
    }
}
