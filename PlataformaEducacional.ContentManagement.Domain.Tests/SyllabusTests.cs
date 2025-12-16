using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.ContentManagement.Domain.ValueObjects;

namespace PlataformaEducacional.ContentManagement.Domain.Tests.ValueObjects
{
    public class SyllabusTests
    {
        [Fact(DisplayName = "Should throw exception when description is empty")]
        [Trait("Category", "Content Management - Syllabus")]
        public void CreateSyllabus_ShouldThrowException_WhenDescriptionIsEmpty()
        {
            // Arrange
            var invalidDescription = string.Empty;

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Syllabus(invalidDescription, 40)
            );

            // Assert
            Assert.Equal("The syllabus description is required.", ex.Message);
        }

        [Fact(DisplayName = "Should throw exception when workload is less than or equal to zero")]
        [Trait("Category", "Content Management - Syllabus")]
        public void CreateSyllabus_ShouldThrowException_WhenWorkloadIsInvalid()
        {
            // Arrange
            var validDescription = "Syllabus description";

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Syllabus(validDescription, 0)
            );

            // Assert
            Assert.Equal("The course workload must be greater than 0.", ex.Message);
        }

        [Fact(DisplayName = "Should throw exception when description exceeds max length")]
        [Trait("Category", "Content Management - Syllabus")]
        public void CreateSyllabus_ShouldThrowException_WhenDescriptionIsTooLong()
        {
            // Arrange
            var longDescription = new string('A', 1001);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Syllabus(longDescription, 40)
            );

            // Assert
            Assert.Equal(
                "The syllabus description must be at most 1000 characters.",
                ex.Message
            );
        }

        [Fact(DisplayName = "Should create syllabus successfully when data is valid")]
        [Trait("Category", "Content Management - Syllabus")]
        public void CreateSyllabus_ShouldCreateSuccessfully_WhenValid()
        {
            // Arrange
            var description = "Syllabus description";
            var workload = 40;

            // Act
            var syllabus = new Syllabus(description, workload);

            // Assert
            Assert.Equal(description, syllabus.Description);
            Assert.Equal(workload, syllabus.Workload);
        }
    }
}
