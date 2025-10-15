using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;

namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries
{
    public interface ICourseQueries
    {
        Task<IReadOnlyList<CourseViewModel>> GetAllAsync(CancellationToken ct);
        Task<IReadOnlyList<CourseViewModel>> GetAvailableWithLessonsAsync(CancellationToken ct);
        Task<CourseViewModel?> GetByIdAsync(Guid courseId, CancellationToken ct);
        Task<CourseViewModel?> GetWithLessonsByIdAsync(Guid courseId, CancellationToken ct);
        Task<IReadOnlyList<LessonViewModel>> GetLessonsByCourseIdAsync(Guid courseId, CancellationToken ct);
        Task<LessonViewModel?> GetLessonByCourseAndIdAsync(Guid courseId, Guid lessonId, CancellationToken ct);
    }
}
