using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.AddLesson
{
    public sealed class AddLessonCommand : Command
    {
        public AddLessonCommand(string title, string content, int order, string? material, Guid courseId)
        {
            AggregateId = courseId;
            CourseId = courseId;
            Title = title;
            Content = content;
            Order = order;
            Material = material;
        }

        public Guid CourseId { get; }
        public string Title { get; }
        public string Content { get; }
        public int Order { get; }
        public string? Material { get; }

        public override bool IsValid()
        {
            ValidationResult = new AddLessonCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
