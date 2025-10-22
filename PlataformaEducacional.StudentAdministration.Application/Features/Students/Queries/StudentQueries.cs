using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries
{
    public class StudentQueries : IStudentQueries
    {
        private readonly IStudentRepository _studentRepository;

        public StudentQueries(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetPendingPaymentEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken)
        {
            var enrollments = await _studentRepository.GetPendingPaymentEnrollmentsByStudentId(studentId, cancellationToken);
            return enrollments.Select(EnrollmentViewModel.FromEnrollment);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetEnrolledStudentsByCourseId(Guid courseId, CancellationToken cancellationToken)
        {
            var enrollments = await _studentRepository.GetEnrolledStudentsByCourseId(courseId, cancellationToken);
            return enrollments.Select(EnrollmentViewModel.FromEnrollment);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetPendingStudentsByCourseId(Guid courseId, CancellationToken cancellationToken)
        {
            var enrollments = await _studentRepository.GetPendingStudentsByCourseId(courseId, cancellationToken);
            return enrollments.Select(EnrollmentViewModel.FromEnrollment);
        }

        public async Task<EnrollmentViewModel?> GetEnrollment(Guid enrollmentId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentRepository.GetEnrollmentWithStudentById(enrollmentId, cancellationToken);
            return enrollment is null ? null : EnrollmentViewModel.FromEnrollment(enrollment);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetActiveEnrollmentsByStudentId(Guid studentId, CancellationToken cancellationToken)
        {
            var enrollments = await _studentRepository.GetActiveEnrollmentsByStudentId(studentId, cancellationToken);
            return enrollments.Select(EnrollmentViewModel.FromEnrollment);
        }

        public async Task<CertificateViewModel?> GetCertificate(Guid enrollmentId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentRepository.GetEnrollmentWithCertificateById(enrollmentId, cancellationToken);
            return enrollment?.Certificate is null ? null : CertificateViewModel.FromEnrollment(enrollment);
        }
    }
}
