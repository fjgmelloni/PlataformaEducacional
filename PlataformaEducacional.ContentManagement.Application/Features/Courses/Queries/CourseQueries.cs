using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;
using PlataformaEducacional.ContentManagement.Domain.Courses;

namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries
{
    public sealed class CourseQueries : ICourseQueries
    {
        private readonly ICourseRepository _repo;

        public CourseQueries(ICourseRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<CourseViewModel>> GetAllAsync(CancellationToken ct)
            => (await _repo.GetAllAsync(ct)).Select(CourseViewModel.FromDomain).ToList();

        public async Task<IReadOnlyList<CourseViewModel>> GetAvailableWithLessonsAsync(CancellationToken ct)
            => (await _repo.GetAvailableWithLessonsAsync(ct)).Select(CourseViewModel.FromDomain).ToList();

        public async Task<CourseViewModel?> GetByIdAsync(Guid courseId, CancellationToken ct)
            => (await _repo.GetByIdAsync(courseId, ct)) is { } c ? CourseViewModel.FromDomain(c) : null;

        public async Task<CourseViewModel?> GetWithLessonsByIdAsync(Guid courseId, CancellationToken ct)
            => (await _repo.GetWithLessonsByIdAsync(courseId, ct)) is { } c ? CourseViewModel.FromDomain(c) : null;

        public async Task<IReadOnlyList<LessonViewModel>> GetLessonsByCourseIdAsync(Guid courseId, CancellationToken ct)
            => (await _repo.GetWithLessonsByIdAsync(courseId, ct))?.Lessons?.OrderBy(l => l.Order).Select(LessonViewModel.FromDomain).ToList()
               ?? new List<LessonViewModel>();

        public async Task<LessonViewModel?> GetLessonByCourseAndIdAsync(Guid courseId, Guid lessonId, CancellationToken ct)
            => (await _repo.GetLessonByCourseAndIdAsync(courseId, lessonId, ct)) is { } l ? LessonViewModel.FromDomain(l) : null;
    }
}
