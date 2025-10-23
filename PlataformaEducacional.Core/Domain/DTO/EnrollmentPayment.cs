namespace PlataformaEducacional.Core.Domain.DTO
{
    public sealed record EnrollmentPayment(
        Guid EnrollmentId,
        Guid StudentId,
        Guid CourseId,
        decimal Amount,
        string CardholderName,
        string CardNumber,
        string CardExpiration,
        string CardCvv
    );
}
