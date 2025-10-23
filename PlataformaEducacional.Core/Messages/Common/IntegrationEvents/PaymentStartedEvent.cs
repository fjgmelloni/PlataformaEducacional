using PlataformaEducacional.Core.Messages.Common.IntegrationEvents;

namespace PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents
{
    public sealed class PaymentStartedEvent : IntegrationEvent
    {
        public Guid EnrollmentId { get; }
        public Guid StudentId { get; }
        public Guid CourseId { get; }
        public decimal Amount { get; }
        public string CardholderName { get; }
        public string CardNumber { get; }
        public string CardExpiration { get; }
        public string CardCvv { get; }

        public PaymentStartedEvent(
            Guid enrollmentId,
            Guid studentId,
            Guid courseId,
            decimal amount,
            string cardholderName,
            string cardNumber,
            string cardExpiration,
            string cardCvv,
            string sourceContext = "Enrollment")
            : base(sourceContext)
        {
            AggregateId = enrollmentId;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            CourseId = courseId;
            Amount = amount;
            CardholderName = cardholderName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardCvv = cardCvv;
        }
    }
}
