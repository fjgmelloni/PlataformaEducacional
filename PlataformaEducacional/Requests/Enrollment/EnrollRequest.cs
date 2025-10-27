using System.ComponentModel.DataAnnotations;

namespace PlataformaEducacional.Api.Requests.Enrollment
{
    public class EnrollRequest
    {
        [Required]
        public Guid CourseId { get; set; }
    }
}
