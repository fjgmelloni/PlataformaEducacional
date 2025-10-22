using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.StudentAdministration.Domain.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task InsertAsync(Student student, CancellationToken cancellationToken);
        Task UpdateEnrollmentAsync(Enrollment enrollment, CancellationToken cancellationToken);
        Task AddLessonProgressAsync(LessonProgress progress, CancellationToken cancellationToken);
        Task<Student?> GetWithEnrollmentsById(Guid studentId, CancellationToken cancellationToken);
        Task EnrollStudentInCourse(Enrollment enrollment, CancellationToken cancellationToken);
        Task<Enrollment?> GetEnrollmentWithStudentById(Guid enrollmentId, CancellationToken cancellationToken);
        Task<Enrollment?> GetEnrollmentWithProgressById(Guid enrollmentId, CancellationToken cancellationToken);
        Task<Enrollment?> GetEnrollmentWithCertificateById(Guid enrollmentId, CancellationToken cancellationToken);
        Task<IEnumerable<Enrollment>> GetPendingPaymentEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken);
        Task<IEnumerable<Enrollment>> GetEnrolledStudentsByCourseId(Guid courseId, CancellationToken cancellationToken);
        Task<IEnumerable<Enrollment>> GetPendingStudentsByCourseId(Guid courseId, CancellationToken cancellationToken);
        Task<IEnumerable<Enrollment>> GetActiveEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken);
        Task GenerateCertificateAsync(Certificate certificate, CancellationToken cancellationToken);
        Task<Enrollment?> GetEnrollmentWithStudentAndCertificateById(Guid enrollmentId, CancellationToken cancellationToken);
    }
}
