using System;

namespace PlataformaEducacao.Core.Domain.DTO
{
    public sealed record EnrollmentPayment(
        Guid EnrollmentId,
        Guid StudentId,
        Guid CourseId,
        decimal TotalAmount,
        string CardHolderName,
        string CardNumber,
        string CardExpiration,
        string CardCvv
    );
}
