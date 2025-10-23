using MediatR;
using PlataformaEducacional.Core.Domain.DTO;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;

namespace PlataformaEducacional.FinancialManagement.Core.Events
{
    public class PaymentEventHandler : INotificationHandler<PaymentStartedEvent>
    {
        private readonly IPaymentService _paymentService;

        public PaymentEventHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Handle(PaymentStartedEvent message, CancellationToken cancellationToken)
        {
            var dto = new EnrollmentPayment(
                message.EnrollmentId,
                message.StudentId,
                message.CourseId,
                message.Amount,
                message.CardholderName,
                message.CardNumber,
                message.CardExpiration,
                message.CardCvv
            );

            await _paymentService.ProcessEnrollmentPayment(dto);
        }
    }
}
