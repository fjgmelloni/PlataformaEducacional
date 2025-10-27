using System.ComponentModel.DataAnnotations;

namespace PlataformaEducacional.Api.Requests.Course
{
    public class AddLessonRequest
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public int Order { get; set; }

        public string? Material { get; set; }
    }
}
