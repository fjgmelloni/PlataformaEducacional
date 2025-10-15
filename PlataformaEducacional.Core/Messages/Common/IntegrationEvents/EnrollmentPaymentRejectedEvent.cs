using PlataformaEducacional.Core.Messages.Common.IntegrationEvents;
using System;

namespace PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents
{
    public sealed class EnrollmentPaymentRejectedEvent : IntegrationEvent
    {
        public Guid EnrollmentId { get; }
        public Guid StudentId { get; }
        public Guid PaymentId { get; }
        public Guid TransactionId { get; }
        public decimal TotalAmount { get; }
        public string Reason { get; }

        public EnrollmentPaymentRejectedEvent(
            Guid enrollmentId,
            Guid studentId,
            Guid paymentId,
            Guid transactionId,
            decimal totalAmount,
            string reason,
            string sourceContext = "Finance")
            : base(sourceContext)
        {
            AggregateId = enrollmentId;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            PaymentId = paymentId;
            TransactionId = transactionId;
            TotalAmount = totalAmount;
            Reason = reason;
        }
    }
}
