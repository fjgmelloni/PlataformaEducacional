using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.ContentManagement.Data.Context;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.ContentManagement.Data.Repositories
{
    public sealed class CourseRepository : ICourseRepository, IDisposable
    {
        private readonly ContentContext _context;

        public CourseRepository(ContentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddAsync(Course course, CancellationToken ct = default)
        {
            await _context.Courses.AddAsync(course, ct).ConfigureAwait(false);
        }

        public Task UpdateAsync(Course course, CancellationToken ct = default)
        {
            _context.Courses.Update(course);
            return Task.CompletedTask;
        }

        public async Task AddLessonAsync(Lesson lesson, CancellationToken ct = default)
        {
            await _context.Lessons.AddAsync(lesson, ct).ConfigureAwait(false);
        }

        public Task<Course?> GetByIdAsync(Guid courseId, CancellationToken ct = default)
            => _context.Courses
                       .AsNoTracking()
                       .FirstOrDefaultAsync(c => c.Id == courseId, ct);

        public Task<Course?> GetByNameAsync(string name, CancellationToken ct = default)
            => _context.Courses
                       .AsNoTracking()
                       .FirstOrDefaultAsync(c => c.Name == name, ct);

        public async Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken ct = default)
            => await _context.Courses
                             .AsNoTracking()
                             .ToListAsync(ct)
                             .ConfigureAwait(false);

        public Task<Course?> GetWithLessonsByIdAsync(Guid courseId, CancellationToken ct = default)
            => _context.Courses
                       .Include(c => c.Lessons)
                       .AsNoTracking()
                       .FirstOrDefaultAsync(c => c.Id == courseId, ct);

        public Task<Lesson?> GetLessonByCourseAndIdAsync(Guid courseId, Guid lessonId, CancellationToken ct = default)
            => _context.Lessons
                       .AsNoTracking()
                       .FirstOrDefaultAsync(a => a.Id == lessonId && a.CourseId == courseId, ct);

        public async Task<IReadOnlyList<Course>> GetAvailableWithLessonsAsync(CancellationToken ct = default)
            => await _context.Courses
                             .Include(c => c.Lessons)
                             .AsNoTracking()
                             .Where(c => c.IsAvailable)
                             .ToListAsync(ct)
                             .ConfigureAwait(false);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
