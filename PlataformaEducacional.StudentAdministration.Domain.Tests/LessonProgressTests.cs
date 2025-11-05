using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Domain.Tests
{
    public class LessonProgressTests
    {
        private readonly Guid _validLessonId = Guid.NewGuid();
        private readonly Guid _validEnrollmentId = Guid.NewGuid();

        [Fact(DisplayName = "Constructor should initialize LessonId and CompletionDate correctly")]
        [Trait("Category", "Student - LessonProgress")]
        public void Constructor_ShouldInitializeLessonIdAndCompletionDate()
        {
            // Arrange
            var before = DateTime.Now.AddSeconds(-1);

            // Act
            var progress = new LessonProgress(_validLessonId);
            var after = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.NotNull(progress);
            Assert.Equal(_validLessonId, progress.LessonId);
            Assert.True(progress.CompletionDate >= before);
            Assert.True(progress.CompletionDate <= after);
            Assert.Equal(Guid.Empty, progress.EnrollmentId);
        }

        [Fact(DisplayName = "Constructor should throw if LessonId is empty")]
        [Trait("Category", "Student - LessonProgress")]
        public void Constructor_ShouldThrow_WhenLessonIdIsEmpty()
        {
            // Arrange
            Guid invalidLessonId = Guid.Empty;

            // Act & Assert
            var exception = Assert.Throws<DomainException>(
                () => new LessonProgress(invalidLessonId)
            );

            Assert.Equal("The lesson ID is required.", exception.Message);
        }

        [Fact(DisplayName = "AssignEnrollment should correctly set EnrollmentId")]
        [Trait("Category", "Student - LessonProgress")]
        public void AssignEnrollment_ShouldSetEnrollmentId_Successfully()
        {
            // Arrange
            var progress = new LessonProgress(_validLessonId);

            // Act
            progress.AssignEnrollment(_validEnrollmentId);

            // Assert
            Assert.Equal(_validEnrollmentId, progress.EnrollmentId);
        }

        [Fact(DisplayName = "AssignEnrollment should throw if EnrollmentId is empty")]
        [Trait("Category", "Student - LessonProgress")]
        public void AssignEnrollment_ShouldThrow_WhenEnrollmentIdIsEmpty()
        {
            // Arrange
            var progress = new LessonProgress(_validLessonId);
            Guid invalidEnrollmentId = Guid.Empty;

            // Act & Assert
            var exception = Assert.Throws<DomainException>(
                () => progress.AssignEnrollment(invalidEnrollmentId)
            );

            Assert.Equal("The enrollment ID is required.", exception.Message);
        }
    }
}
