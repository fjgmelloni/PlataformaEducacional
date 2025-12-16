using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.ContentManagement.Domain.Lessons;

namespace PlataformaEducacional.ContentManagement.Domain.Tests.Lessons
{
    public class LessonTests
    {
        [Fact(DisplayName = "Should throw exception when lesson title is invalid")]
        [Trait("Category", "Content Management - Lesson")]
        public void CreateLesson_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange
            var invalidTitle = string.Empty;
            var validContent = "Lesson content";
            var validOrder = 1;
            var material = "Lesson material";

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Lesson(invalidTitle, validContent, validOrder, material)
            );

            // Assert
            Assert.Equal("The lesson title is required.", ex.Message);
        }

        [Fact(DisplayName = "Should throw exception when lesson content is invalid")]
        [Trait("Category", "Content Management - Lesson")]
        public void CreateLesson_ShouldThrowException_WhenContentIsEmpty()
        {
            // Arrange
            var validTitle = "Data Structures";
            var invalidContent = string.Empty;
            var validOrder = 1;
            var material = "Lesson material";

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Lesson(validTitle, invalidContent, validOrder, material)
            );

            // Assert
            Assert.Equal("The lesson content is required.", ex.Message);
        }

        [Fact(DisplayName = "Should create valid lesson with material")]
        [Trait("Category", "Content Management - Lesson")]
        public void CreateLesson_ShouldCreateSuccessfully_WhenValidWithMaterial()
        {
            // Arrange
            var title = "Data Structures";
            var content = "Lesson content";
            var order = 1;
            var material = "Lesson material";

            // Act
            var lesson = new Lesson(title, content, order, material);

            // Assert
            Assert.Equal(title, lesson.Title);
            Assert.Equal(content, lesson.Content);
            Assert.Equal(order, lesson.Order);
            Assert.Equal(material, lesson.Material);
        }

        [Fact(DisplayName = "Should create valid lesson without material")]
        [Trait("Category", "Content Management - Lesson")]
        public void CreateLesson_ShouldCreateSuccessfully_WhenValidWithoutMaterial()
        {
            // Arrange
            var title = "Data Structures";
            var content = "Lesson content";
            var order = 1;

            // Act
            var lesson = new Lesson(title, content, order, null);

            // Assert
            Assert.Equal(title, lesson.Title);
            Assert.Equal(content, lesson.Content);
            Assert.Equal(order, lesson.Order);
            Assert.Null(lesson.Material);
        }
    }
}
