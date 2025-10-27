using System.ComponentModel.DataAnnotations;

namespace PlataformaEducacional.Api.Requests.Enrollment
{
    public class ProcessPaymentRequest
    {
        [Required]
        public Guid EnrollmentId { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public string CardName { get; set; } = null!;

        [Required]
        public string CardNumber { get; set; } = null!;

        [Required]
        public string CardExpiration { get; set; } = null!;

        [Required]
        public string CardCvv { get; set; } = null!;
    }
}
