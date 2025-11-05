using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Domain.Tests
{
    public class EnrollmentTests
    {
        private readonly Guid _courseId = Guid.NewGuid();
        private const string _courseName = "C# Course";
        private const int _totalLessons = 10;
        private const decimal _value = 500;

        private Enrollment CreateEnrollment()
        {
            return new Enrollment(_courseId, _courseName, _totalLessons, _value);
        }

        [Fact(DisplayName = nameof(Constructor_ValidEnrollment))]
        [Trait("Category", "Enrollment - Constructor")]
        public void Constructor_ValidEnrollment()
        {
            var enrollment = CreateEnrollment();

            Assert.Equal(_courseName, enrollment.CourseName);
            Assert.Equal(_courseId, enrollment.CourseId);
            Assert.Equal(_value, enrollment.Value);
            Assert.Equal(_totalLessons, enrollment.LearningHistory.TotalLessons);
        }

        [Fact(DisplayName = nameof(Constructor_InvalidCourseId))]
        [Trait("Category", "Enrollment - Constructor")]
        public void Constructor_InvalidCourseId()
        {
            var invalidId = Guid.Empty;

            var ex = Assert.Throws<DomainException>(() => new Enrollment(invalidId, _courseName, _totalLessons, _value));

            Assert.Equal("Course ID is required.", ex.Message);
        }

        [Fact(DisplayName = nameof(Constructor_InvalidTotalLessons))]
        [Trait("Category", "Enrollment - Constructor")]
        public void Constructor_InvalidTotalLessons()
        {
            int invalidLessons = 0;

            var ex = Assert.Throws<DomainException>(() => new Enrollment(_courseId, _courseName, invalidLessons, _value));

            Assert.Equal("The course must have more than 0 lessons.", ex.Message);
        }

        [Fact(DisplayName = nameof(AssignStudent_Valid))]
        [Trait("Category", "Enrollment - Student Association")]
        public void AssignStudent_Valid()
        {
            var studentId = Guid.NewGuid();
            var enrollment = CreateEnrollment();

            enrollment.AssignStudent(studentId);

            Assert.Equal(studentId, enrollment.StudentId);
        }

        [Fact(DisplayName = "IsActive should return false when status is PendingPayment")]
        [Trait("Category", "Enrollment - State")]
        public void IsActive_WhenPending_ReturnsFalse()
        {
            var enrollment = CreateEnrollment();

            Assert.False(enrollment.IsActive());
        }

        [Fact(DisplayName = "IsActive should return true when status is Active")]
        [Trait("Category", "Enrollment - State")]
        public void IsActive_WhenActive_ReturnsTrue()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();

            Assert.True(enrollment.IsActive());
        }

        [Fact(DisplayName = "Activate should change status to Active")]
        [Trait("Category", "Enrollment - State")]
        public void Activate_ChangesStatus()
        {
            var enrollment = CreateEnrollment();

            enrollment.Activate();

            Assert.Equal(EnrollmentStatus.Active, enrollment.EnrollmentStatus);
        }

        [Fact(DisplayName = "Deactivate should change status to PendingPayment")]
        [Trait("Category", "Enrollment - State")]
        public void Deactivate_ChangesStatus()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();

            enrollment.Deactivate();

            Assert.Equal(EnrollmentStatus.PendingPayment, enrollment.EnrollmentStatus);
        }

        [Fact(DisplayName = "LessonAlreadyCompleted should return false for new lesson")]
        [Trait("Category", "Enrollment - Lessons")]
        public void LessonAlreadyCompleted_ReturnsFalse_WhenNotRecorded()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();

            var lesson = new LessonProgress(Guid.NewGuid());
            enrollment.RecordLesson(lesson);

            var newLesson = new LessonProgress(Guid.NewGuid());

            Assert.False(enrollment.LessonAlreadyCompleted(newLesson));
        }

        [Fact(DisplayName = "LessonAlreadyCompleted should return true for recorded lesson")]
        [Trait("Category", "Enrollment - Lessons")]
        public void LessonAlreadyCompleted_ReturnsTrue_WhenRecorded()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();

            var lesson = new LessonProgress(Guid.NewGuid());
            enrollment.RecordLesson(lesson);

            Assert.True(enrollment.LessonAlreadyCompleted(lesson));
        }

        [Fact(DisplayName = "RecordLesson should add lesson and update progress")]
        [Trait("Category", "Enrollment - Lessons")]
        public void RecordLesson_ShouldAddAndUpdateProgress()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();
            var lesson = new LessonProgress(Guid.NewGuid());

            enrollment.RecordLesson(lesson);

            Assert.Single(enrollment.LessonProgresses);
            Assert.Equal(enrollment.Id, lesson.EnrollmentId);
            Assert.Equal(10, enrollment.LearningHistory.OverallProgress);
        }

        [Fact(DisplayName = "RecordLesson should throw when enrollment is not Active")]
        [Trait("Category", "Enrollment - Lessons")]
        public void RecordLesson_PendingEnrollment_ThrowsException()
        {
            var enrollment = CreateEnrollment();
            var lesson = new LessonProgress(Guid.NewGuid());

            var ex = Assert.Throws<DomainException>(() => enrollment.RecordLesson(lesson));

            Assert.Equal("Enrollment pending payment.", ex.Message);
        }

        [Fact(DisplayName = "RecordLesson should throw when lesson already recorded")]
        [Trait("Category", "Enrollment - Lessons")]
        public void RecordLesson_AlreadyCompleted_ThrowsException()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();
            var lesson = new LessonProgress(Guid.NewGuid());

            enrollment.RecordLesson(lesson);

            var ex = Assert.Throws<DomainException>(() => enrollment.RecordLesson(lesson));

            Assert.Equal("Lesson already recorded.", ex.Message);
            Assert.Single(enrollment.LessonProgresses);
        }

        [Fact(DisplayName = "CompleteCourse should finalize when progress is 100")]
        [Trait("Category", "Enrollment - Completion")]
        public void CompleteCourse_Valid()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();

            for (int i = 0; i < _totalLessons; i++)
                enrollment.RecordLesson(new LessonProgress(Guid.NewGuid()));

            enrollment.CompleteCourse();

            Assert.Equal(100, enrollment.LearningHistory.OverallProgress);
        }

        [Fact(DisplayName = "CompleteCourse should throw when enrollment pending")]
        [Trait("Category", "Enrollment - Completion")]
        public void CompleteCourse_PendingEnrollment_ThrowsException()
        {
            var enrollment = CreateEnrollment();

            var ex = Assert.Throws<DomainException>(() => enrollment.CompleteCourse());

            Assert.Equal("Enrollment pending payment.", ex.Message);
        }

        [Fact(DisplayName = "CompleteCourse should throw when progress < 100")]
        [Trait("Category", "Enrollment - Completion")]
        public void CompleteCourse_IncompleteProgress_ThrowsException()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();

            for (int i = 0; i < _totalLessons - 1; i++)
                enrollment.RecordLesson(new LessonProgress(Guid.NewGuid()));

            var ex = Assert.Throws<DomainException>(() => enrollment.CompleteCourse());

            Assert.Equal("There are pending lessons to complete.", ex.Message);
        }

        [Fact(DisplayName = "CompleteCourse should throw when course already completed")]
        [Trait("Category", "Enrollment - Completion")]
        public void CompleteCourse_AlreadyCompleted_ThrowsException()
        {
            var enrollment = CreateEnrollment();
            enrollment.Activate();

            for (int i = 0; i < _totalLessons; i++)
                enrollment.RecordLesson(new LessonProgress(Guid.NewGuid()));

            enrollment.CompleteCourse();

            var ex = Assert.Throws<DomainException>(() => enrollment.CompleteCourse());

            Assert.Equal("Course already completed.", ex.Message);
        }
    }
}
