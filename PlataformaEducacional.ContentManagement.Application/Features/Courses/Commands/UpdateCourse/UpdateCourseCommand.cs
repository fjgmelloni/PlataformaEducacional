using PlataformaEducacional.Core.Messages.Base;


namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.UpdateCourse
{
    public sealed class UpdateCourseCommand : Command
    {
        public UpdateCourseCommand(Guid courseId, string name, string syllabusDescription, int syllabusWorkload, decimal price, bool isAvailable)
        {
            AggregateId = courseId;
            CourseId = courseId;
            Name = name;
            SyllabusDescription = syllabusDescription;
            SyllabusWorkload = syllabusWorkload;
            Price = price;
            IsAvailable = isAvailable;
        }

        public Guid CourseId { get; }
        public string Name { get; }
        public string SyllabusDescription { get; }
        public int SyllabusWorkload { get; }
        public decimal Price { get; }
        public bool IsAvailable { get; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCourseCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
