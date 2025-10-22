using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Core.Data;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentAdministrationContext _context;

        public StudentRepository(StudentAdministrationContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task InsertAsync(Student student, CancellationToken cancellationToken)
        {
            await _context.Students.AddAsync(student, cancellationToken);
        }

        public Task UpdateEnrollmentAsync(Enrollment enrollment, CancellationToken cancellationToken)
        {
            _context.Enrollments.Update(enrollment);
            return Task.CompletedTask;
        }

        public async Task AddLessonProgressAsync(LessonProgress progress, CancellationToken cancellationToken)
        {
            await _context.LessonProgresses.AddAsync(progress, cancellationToken);
        }

        public async Task GenerateCertificateAsync(Certificate certificate, CancellationToken cancellationToken)
        {
            await _context.Certificates.AddAsync(certificate, cancellationToken);
        }

        public async Task<IEnumerable<Enrollment>> GetPendingPaymentEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.LearningHistory)
                .AsNoTracking()
                .Where(e => e.EnrollmentStatus == EnrollmentStatus.PendingPayment &&
                            e.StudentId == studentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrolledStudentsByCourseId(Guid courseId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.LearningHistory)
                .AsNoTracking()
                .Where(e => e.EnrollmentStatus == EnrollmentStatus.Active &&
                            e.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Enrollment>> GetPendingStudentsByCourseId(Guid courseId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.LearningHistory)
                .AsNoTracking()
                .Where(e => e.EnrollmentStatus != EnrollmentStatus.Active &&
                            e.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Student?> GetWithEnrollmentsById(Guid studentId, CancellationToken cancellationToken)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);
        }

        public async Task<Enrollment?> GetEnrollmentWithStudentById(Guid enrollmentId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments
                .Include(e => e.LearningHistory)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken);
        }

        public async Task<Enrollment?> GetEnrollmentWithCertificateById(Guid enrollmentId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments
                .Include(e => e.LearningHistory)
                .Include(e => e.Student)
                .Include(e => e.Certificate)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken);
        }

        public async Task<Enrollment?> GetEnrollmentWithProgressById(Guid enrollmentId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments
                .Include(e => e.LearningHistory)
                .Include(e => e.LessonProgresses)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken);
        }

        public async Task<IEnumerable<Enrollment>> GetActiveEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.LearningHistory)
                .AsNoTracking()
                .Where(e => e.EnrollmentStatus == EnrollmentStatus.Active &&
                            e.StudentId == studentId)
                .ToListAsync(cancellationToken);
        }

        public async Task EnrollStudentInCourse(Enrollment enrollment, CancellationToken cancellationToken)
        {
            await _context.Enrollments.AddAsync(enrollment, cancellationToken);
        }

        public void Dispose() { }
    }
}
