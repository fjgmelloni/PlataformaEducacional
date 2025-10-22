using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries
{
    public interface IStudentQueries
    {
        Task<EnrollmentViewModel?> GetEnrollment(Guid enrollmentId, CancellationToken cancellationToken);
        Task<IEnumerable<EnrollmentViewModel>> GetPendingPaymentEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken);
        Task<IEnumerable<EnrollmentViewModel>> GetActiveEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken);
        Task<IEnumerable<EnrollmentViewModel>> GetEnrolledStudentsByCourseId(Guid courseId, CancellationToken cancellationToken);
        Task<IEnumerable<EnrollmentViewModel>> GetPendingStudentsByCourseId(Guid courseId, CancellationToken cancellationToken);
        Task<CertificateViewModel?> GetCertificate(Guid enrollmentId, CancellationToken cancellationToken);
    }
}
