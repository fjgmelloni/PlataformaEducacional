using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.EnrollInCourse
{
    public class EnrollInCourseCommandTests
    {
        [Fact(DisplayName = "Command should be valid when all fields are correct")]
        [Trait("Category", "EnrollInCourseCommand")]
        public void Should_Be_Valid()
        {
            // Arrange
            var command = new EnrollInCourseCommand(Guid.NewGuid(), Guid.NewGuid(), "C# Course", 5, 500);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should be invalid when StudentId is empty")]
        [Trait("Category", "EnrollInCourseCommand")]
        public void Should_Be_Invalid_When_StudentId_Empty()
        {
            var command = new EnrollInCourseCommand(Guid.Empty, Guid.NewGuid(), "C# Course", 5, 500);

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains("O ID do aluno é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when CourseId is empty")]
        [Trait("Category", "EnrollInCourseCommand")]
        public void Should_Be_Invalid_When_CourseId_Empty()
        {
            var command = new EnrollInCourseCommand(Guid.NewGuid(), Guid.Empty, "C# Course", 5, 500);

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains("O ID do curso é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when CourseName is empty")]
        [Trait("Category", "EnrollInCourseCommand")]
        public void Should_Be_Invalid_When_CourseName_Empty()
        {
            var command = new EnrollInCourseCommand(Guid.NewGuid(), Guid.NewGuid(), "", 5, 500);

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains("O nome do curso é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when TotalLessons is zero or less")]
        [Trait("Category", "EnrollInCourseCommand")]
        public void Should_Be_Invalid_When_TotalLessons_Invalid()
        {
            var command = new EnrollInCourseCommand(Guid.NewGuid(), Guid.NewGuid(), "C# Course", 0, 500);

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains("O curso deve ter pelo menos uma aula.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when Value is zero or less")]
        [Trait("Category", "EnrollInCourseCommand")]
        public void Should_Be_Invalid_When_Value_Invalid()
        {
            var command = new EnrollInCourseCommand(Guid.NewGuid(), Guid.NewGuid(), "C# Course", 5, 0);

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains("O valor do curso deve ser maior que zero.", command.ValidationResult.Errors);
        }
    }
}
