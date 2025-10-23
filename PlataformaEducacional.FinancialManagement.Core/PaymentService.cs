using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain.DTO;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.FinancialManagement.Core
{
    public class PaymentService : IPaymentService
    {
        private readonly ICreditCardPaymentFacade _creditCardFacade;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediatorHandler _mediator;

        public PaymentService(
            ICreditCardPaymentFacade creditCardFacade,
            IPaymentRepository paymentRepository,
            IMediatorHandler mediator)
        {
            _creditCardFacade = creditCardFacade;
            _paymentRepository = paymentRepository;
            _mediator = mediator;
        }

        public async Task<Transaction> ProcessEnrollmentPayment(EnrollmentPayment request)
        {
            // Map DTO → Domain
            var card = new CardData(
                request.CardholderName,
                request.CardNumber,
                request.CardExpiration,
                request.CardCvv);

            var payment = new Payment(request.EnrollmentId, request.Amount, card);
            var transaction = _creditCardFacade.Charge(request.EnrollmentId, payment);

            _paymentRepository.Add(payment);
            _paymentRepository.AddTransaction(transaction);

            if (transaction.Status == TransactionStatus.Paid)
            {
                await _mediator.PublishEventAsync(new EnrollmentPaymentCompletedEvent(
                    request.EnrollmentId,
                    request.StudentId,
                    payment.Id,
                    transaction.Id,
                    request.Amount));
            }
            else
            {
                await _mediator.PublishNotificationAsync(new DomainNotification("payment", "Payment was declined."));
                await _mediator.PublishEventAsync(new EnrollmentPaymentRejectedEvent(
                    request.EnrollmentId,
                    request.StudentId,
                    payment.Id,
                    transaction.Id,
                    request.Amount,
                    "Payment was declined."));
            }

            return transaction;
        }
    }
}
