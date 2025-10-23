using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels
{
    public class EnrollmentViewModel
    {
        public EnrollmentViewModel(Guid enrollmentId, Guid studentId, string studentName, Guid courseId, string courseName, EnrollmentStatus enrollmentStatus, DateTime enrollmentDate, CourseStatus courseStatus, DateTime? completionDate, double courseProgress)
        {
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            StudentName = studentName;
            CourseId = courseId;
            CourseName = courseName;
            EnrollmentStatus = enrollmentStatus;
            EnrollmentDate = enrollmentDate;
            CourseStatus = courseStatus;
            CompletionDate = completionDate;
            CourseProgress = courseProgress;
        }

        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public CourseStatus CourseStatus { get; set; }
        public DateTime? CompletionDate { get; set; }
        public double CourseProgress { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public static EnrollmentViewModel FromEnrollment(Enrollment enrollment)
            => new(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.Student.Name,
                enrollment.CourseId,
                enrollment.CourseName,
                enrollment.EnrollmentStatus,
                enrollment.EnrollmentDate,
                enrollment.LearningHistory.CourseStatus,
                enrollment.LearningHistory.CompletionDate,
                enrollment.LearningHistory.OverallProgress

            );
    }
}
