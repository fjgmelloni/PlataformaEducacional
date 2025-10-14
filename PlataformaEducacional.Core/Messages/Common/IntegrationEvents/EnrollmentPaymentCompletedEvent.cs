using PlataformaEducacional.Core.Messages.Common.IntegrationEvents;
using System;

namespace PlataformaEducacao.Core.Messages.CommonMessages.IntegrationEvents
{
    public sealed class EnrollmentPaymentCompletedEvent : IntegrationEvent
    {
        public Guid EnrollmentId { get; }
        public Guid StudentId { get; }
        public Guid PaymentId { get; }
        public Guid TransactionId { get; }
        public decimal TotalAmount { get; }

        public EnrollmentPaymentCompletedEvent(
            Guid enrollmentId,
            Guid studentId,
            Guid paymentId,
            Guid transactionId,
            decimal totalAmount,
            string sourceContext = "Finance")
            : base(sourceContext)
        {
            AggregateId = enrollmentId;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            PaymentId = paymentId;
            TransactionId = transactionId;
            TotalAmount = totalAmount;
        }
    }
}
