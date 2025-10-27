using System.ComponentModel.DataAnnotations;

namespace PlataformaEducacional.Api.Requests.Course
{
    public class AddCourseRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string ContentDescription { get; set; } = null!;

        [Required]
        public int Workload { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool Available { get; set; }
    }
}
