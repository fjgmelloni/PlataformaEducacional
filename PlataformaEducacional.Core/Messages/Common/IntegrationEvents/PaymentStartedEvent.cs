using PlataformaEducacional.Core.Messages.Common.IntegrationEvents;
using System;

namespace PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents
{
    public sealed class PaymentStartedEvent : IntegrationEvent
    {
        public Guid EnrollmentId { get; }
        public Guid CourseId { get; }
        public Guid StudentId { get; }
        public decimal Amount { get; }
        public string CardHolderName { get; }
        public string CardNumber { get; }
        public string CardExpiration { get; }
        public string CardCvv { get; }

        public PaymentStartedEvent(
            Guid enrollmentId,
            Guid courseId,
            Guid studentId,
            decimal amount,
            string cardHolderName,
            string cardNumber,
            string cardExpiration,
            string cardCvv,
            string sourceContext = "Enrollment")
            : base(sourceContext)
        {
            AggregateId = enrollmentId;
            EnrollmentId = enrollmentId;
            CourseId = courseId;
            StudentId = studentId;
            Amount = amount;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardCvv = cardCvv;
        }
    }
}
