using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Domain.Tests
{
    public class LearningHistoryTests
    {
        private const int ValidTotalLessons = 10;
        private const double ValidProgress = 50.0;
        private const double CompleteProgress = 100.0;

        [Fact(DisplayName = "PendingPayment should create history as NotStarted and progress zero")]
        [Trait("Category", "LearningHistory - Factory")]
        public void LearningHistoryFactory_PendingPayment_ShouldCreateCorrectState()
        {
            // Arrange & Act
            var history = LearningHistory.Factory.PendingPayment(ValidTotalLessons);

            // Assert
            Assert.NotNull(history);
            Assert.Equal(ValidTotalLessons, history.TotalLessons);
            Assert.Equal(0.0, history.OverallProgress);
            Assert.Equal(CourseStatus.NotStarted, history.CourseStatus);
            Assert.Null(history.CompletionDate);
        }

        [Theory(DisplayName = "PendingPayment should throw exception if total lessons is invalid")]
        [Trait("Category", "LearningHistory - Validation")]
        [InlineData(0)]
        [InlineData(-5)]
        public void LearningHistoryFactory_PendingPayment_ShouldThrow_WhenInvalidTotalLessons(int totalLessons)
        {
            // Act
            Action act = () => LearningHistory.Factory.PendingPayment(totalLessons);

            // Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Equal("The course must have more than 0 lessons.", ex.Message);
        }

        [Fact(DisplayName = "InProgress should create history as InProgress with progress set")]
        [Trait("Category", "LearningHistory - Factory")]
        public void LearningHistoryFactory_InProgress_ShouldCreateCorrectState()
        {
            // Arrange & Act
            var history = LearningHistory.Factory.InProgress(ValidTotalLessons, ValidProgress);

            // Assert
            Assert.NotNull(history);
            Assert.Equal(ValidTotalLessons, history.TotalLessons);
            Assert.Equal(ValidProgress, history.OverallProgress);
            Assert.Equal(CourseStatus.InProgress, history.CourseStatus);
            Assert.Null(history.CompletionDate);
        }

        [Fact(DisplayName = "Completed should create history as Completed and set CompletionDate")]
        [Trait("Category", "LearningHistory - Factory")]
        public void LearningHistoryFactory_Completed_ShouldCreateCorrectState()
        {
            // Arrange & Act
            var history = LearningHistory.Factory.Completed(ValidTotalLessons, CompleteProgress);

            // Assert
            Assert.NotNull(history);
            Assert.Equal(ValidTotalLessons, history.TotalLessons);
            Assert.Equal(CompleteProgress, history.OverallProgress);
            Assert.Equal(CourseStatus.Completed, history.CourseStatus);
            Assert.NotNull(history.CompletionDate);
            Assert.True(history.CompletionDate.Value >= DateTime.Now.AddSeconds(-1));
        }

        [Theory(DisplayName = "Completed should throw when total lessons is invalid")]
        [Trait("Category", "LearningHistory - Validation")]
        [InlineData(0)]
        [InlineData(-1)]
        public void LearningHistoryFactory_Completed_ShouldThrow_WhenInvalidTotalLessons(int totalLessons)
        {
            // Act
            Action act = () => LearningHistory.Factory.Completed(totalLessons, CompleteProgress);

            // Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Equal("The course must have more than 0 lessons.", ex.Message);
        }
    }
}
