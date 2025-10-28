using FluentValidation;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PaymentEnrollment
{
    public sealed class PaymentEnrollmentCommand : Command
    {
        public PaymentEnrollmentCommand(Guid enrollmentId, Guid studentId, decimal total, string cardName, string cardNumber, string cardExpiration, string cardCvv)
        {
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            Total = total;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardCvv = cardCvv;
        }

        public Guid EnrollmentId { get; }
        public Guid StudentId { get; }
        public decimal Total { get; }
        public string CardName { get; }
        public string CardNumber { get; }
        public string CardExpiration { get; }
        public string CardCvv { get; }

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
            RuleFor(c => c.EnrollmentId)
                .NotEmpty().WithMessage("O ID da matrícula é obrigatório.");

            RuleFor(c => c.StudentId)
                .NotEmpty().WithMessage("O ID do aluno é obrigatório.");

            RuleFor(c => c.Total)
                .GreaterThan(0).WithMessage("O valor do pagamento deve ser maior que zero.");

            RuleFor(c => c.CardName)
                .NotEmpty().WithMessage("O nome do titular do cartão é obrigatório.");

            RuleFor(c => c.CardNumber)
                .CreditCard().WithMessage("O número do cartão é inválido.");

            RuleFor(c => c.CardExpiration)
                .NotEmpty().WithMessage("A data de expiração do cartão é obrigatória.");

            RuleFor(c => c.CardCvv)
                .NotEmpty().WithMessage("O código de segurança (CVV) é obrigatório.");
        }
    }
}
