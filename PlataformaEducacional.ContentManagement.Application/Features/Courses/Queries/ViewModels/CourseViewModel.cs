// Features/Courses/Queries/ViewModels/CourseViewModel.cs
namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels
{
    public sealed class CourseViewModel
    {
        public CourseViewModel(Guid id, string name, string syllabusDescription, int syllabusWorkload, decimal price, bool isAvailable, IEnumerable<LessonViewModel> lessons)
        {
            Id = id;
            Name = name;
            SyllabusDescription = syllabusDescription;
            SyllabusWorkload = syllabusWorkload;
            Price = price;
            IsAvailable = isAvailable;
            Lessons = lessons ?? Enumerable.Empty<LessonViewModel>();
        }

        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string SyllabusDescription { get; init; } = null!;
        public int SyllabusWorkload { get; init; }
        public decimal Price { get; init; }
        public bool IsAvailable { get; init; }
        public IEnumerable<LessonViewModel> Lessons { get; init; }

        public static CourseViewModel FromDomain(Domain.Courses.Course course) =>
         new(course.Id,
             course.Name,
             course.Syllabus.Description ?? string.Empty,
             course.Syllabus.Workload, // << corrigido aqui
             course.Price,
             course.IsAvailable,
             (course.Lessons ?? Array.Empty<Domain.Lessons.Lesson>())
                 .OrderBy(l => l.Order)
                 .Select(LessonViewModel.FromDomain));
    }
}
