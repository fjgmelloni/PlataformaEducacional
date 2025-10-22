using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.StudentAdministration.Domain
{
    public class LessonProgress : Entity
    {
        public Guid EnrollmentId { get; private set; }
        public Guid LessonId { get; private set; }
        public DateTime CompletionDate { get; private set; }
        public Enrollment Enrollment { get; private set; } = null!;

        protected LessonProgress() { }

        public LessonProgress(Guid lessonId)
        {
            LessonId = lessonId;
            CompletionDate = DateTime.Now;
            Validations.ValidateIfEmpty(LessonId, "Lesson ID is required.");
        }

        public void AssignEnrollment(Guid enrollmentId)
        {
            EnrollmentId = enrollmentId;
            Validations.ValidateIfEmpty(EnrollmentId, "Enrollment ID is required.");
        }
    }
}
