using FluentValidation;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PaymentEnrollment
{
    public sealed class PaymentEnrollmentCommand : Command
    {
        public PaymentEnrollmentCommand(Guid enrollmentId, Guid studentId)
        {
            EnrollmentId = enrollmentId;
            StudentId = studentId;
        }

        public Guid EnrollmentId { get; }
        public Guid StudentId { get; }

        public override bool IsValid()
        {
            var result = new PaymentEnrollmentCommandValidator().Validate(this);
            ValidationResult.Errors.Clear();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ValidationResult.AddError(error.ErrorMessage);
            }
            return ValidationResult.IsValid;
        }

    }

    public sealed class PaymentEnrollmentCommandValidator : AbstractValidator<PaymentEnrollmentCommand>
    {
        public PaymentEnrollmentCommandValidator()
        {
            RuleFor(c => c.StudentId)
                .NotEmpty().WithMessage("O ID do aluno é obrigatório.");

            RuleFor(c => c.EnrollmentId)
                .NotEmpty().WithMessage("O ID da matrícula é obrigatório.");
        }
    }
}
