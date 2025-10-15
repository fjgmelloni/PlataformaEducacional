using PlataformaEducacional.Core.Data;
using PlataformaEducacional.ContentManagement.Domain.Lessons;

namespace PlataformaEducacional.ContentManagement.Domain.Courses
{
    public interface ICourseRepository : IRepository<Course>, IDisposable
    {
        Task AddAsync(Course course, CancellationToken ct);
        Task AddLessonAsync(Lesson lesson, CancellationToken ct);
        Task UpdateAsync(Course course, CancellationToken ct);
        Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken ct);
        Task<IReadOnlyList<Course>> GetAvailableWithLessonsAsync(CancellationToken ct);
        Task<Course?> GetByIdAsync(Guid courseId, CancellationToken ct);
        Task<Course?> GetByNameAsync(string name, CancellationToken ct);
        Task<Lesson?> GetLessonByCourseAndIdAsync(Guid courseId, Guid lessonId, CancellationToken ct);
        Task<Course?> GetWithLessonsByIdAsync(Guid courseId, CancellationToken ct);
    }
}
