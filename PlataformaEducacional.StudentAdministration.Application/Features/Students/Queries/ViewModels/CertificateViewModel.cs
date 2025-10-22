using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels
{
    public class CertificateViewModel
    {
        public CertificateViewModel(string studentName, string courseName, DateTime completionDate, string verificationCode)
        {
            StudentName = studentName;
            CourseName = courseName;
            CompletionDate = completionDate;
            VerificationCode = verificationCode;
        }

        public string StudentName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public DateTime CompletionDate { get; set; }
        public string VerificationCode { get; set; } = null!;

        public static CertificateViewModel FromEnrollment(Enrollment enrollment)
            => new(
                enrollment.Student.Name,
                enrollment.CourseName,
                enrollment.LearningHistory.CompletionDate!.Value,
                enrollment.Certificate!.VerificationCode
            );
    }
}
