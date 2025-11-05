using PlataformaEducacional.StudentAdministration.Application.Features.Students.Events;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Events
{
    public class CourseCompletedEventTests
    {
        [Fact(DisplayName = "CourseCompletedEvent should assign EnrollmentId correctly")]
        public void CourseCompletedEvent_Should_Set_EnrollmentId()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();

            // Act
            var @event = new CourseCompletedEvent(enrollmentId);

            // Assert
            Assert.Equal(enrollmentId, @event.EnrollmentId);
        }
    }
}
