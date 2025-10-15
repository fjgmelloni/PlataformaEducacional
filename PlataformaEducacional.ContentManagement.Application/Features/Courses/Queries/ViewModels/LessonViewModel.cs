// Features/Courses/Queries/ViewModels/LessonViewModel.cs
namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels
{
    public sealed class LessonViewModel
    {
        public LessonViewModel(Guid id, Guid courseId, string title, string content, int order, string? material)
        {
            Id = id;
            CourseId = courseId;
            Title = title;
            Content = content;
            Order = order;
            Material = material;
        }

        public Guid Id { get; init; }
        public Guid CourseId { get; init; }
        public string Title { get; init; } = null!;
        public string Content { get; init; } = null!;
        public int Order { get; init; }
        public string? Material { get; init; }

        public static LessonViewModel FromDomain(Domain.Lessons.Lesson lesson) =>
            new(lesson.Id, lesson.CourseId, lesson.Title, lesson.Content, lesson.Order, lesson.Material);
    }
}
