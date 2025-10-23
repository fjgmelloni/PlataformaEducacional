using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Events
{
    public class CourseCompletedEvent : Event
    {
        public Guid EnrollmentId { get; private set; }

        public CourseCompletedEvent(Guid enrollmentId)
        {
            EnrollmentId = enrollmentId;
        }
    }
}
