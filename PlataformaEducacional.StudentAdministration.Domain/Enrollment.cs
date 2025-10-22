using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.StudentAdministration.Domain
{
    public class Enrollment : Entity
    {
        public Guid CourseId { get; private set; }
        public string CourseName { get; private set; } = null!;
        public decimal Value { get; private set; }
        public Guid StudentId { get; private set; }
        public DateTime EnrollmentDate { get; private set; }
        public EnrollmentStatus EnrollmentStatus { get; private set; }
        public LearningHistory LearningHistory { get; private set; } = null!;
        public Student Student { get; private set; } = null!;
        public Certificate? Certificate { get; set; }

        private readonly List<LessonProgress> _lessonProgresses;
        public IReadOnlyCollection<LessonProgress> LessonProgresses => _lessonProgresses;

        protected Enrollment()
        {
            _lessonProgresses = new List<LessonProgress>();
        }

        public Enrollment(Guid courseId, string courseName, int totalLessons, decimal value)
        {
            CourseId = courseId;
            CourseName = courseName;
            Value = value;
            EnrollmentDate = DateTime.Now;
            EnrollmentStatus = EnrollmentStatus.PendingPayment;
            LearningHistory = LearningHistory.Factory.PendingPayment(totalLessons);
            _lessonProgresses = new List<LessonProgress>();
            Validate();
        }

        public void AssignStudent(Guid studentId)
        {
            StudentId = studentId;
        }

        public void LinkStudent(Student student)
        {
            Student = student;
        }

        public bool IsActive() => EnrollmentStatus == EnrollmentStatus.Active;

        public void Activate() => EnrollmentStatus = EnrollmentStatus.Active;

        public void Deactivate() => EnrollmentStatus = EnrollmentStatus.PendingPayment;

        public void StartPayment() => EnrollmentStatus = EnrollmentStatus.ProcessingPayment;

        public bool LessonAlreadyCompleted(LessonProgress progress)
            => _lessonProgresses.Any(p => p.LessonId == progress.LessonId);

        public void RecordLesson(LessonProgress progress)
        {
            if (!IsActive())
                throw new DomainException("Enrollment pending payment.");

            if (LessonAlreadyCompleted(progress))
                throw new DomainException("Lesson already recorded.");

            progress.AssignEnrollment(Id);
            _lessonProgresses.Add(progress);

            UpdateCourseProgress();
        }

        private void UpdateCourseProgress()
        {
            int totalLessons = LearningHistory.TotalLessons;
            int completedLessons = _lessonProgresses.Count;

            double newProgress = (double)completedLessons / totalLessons * 100;
            LearningHistory = LearningHistory.Factory.InProgress(totalLessons, newProgress);
        }

        public void CompleteCourse()
        {
            if (!IsActive())
                throw new DomainException("Enrollment pending payment.");

            if (LearningHistory.OverallProgress < 100)
                throw new DomainException("There are pending lessons to complete.");

            if (LearningHistory.CourseStatus == CourseStatus.Completed)
                throw new DomainException("Course already completed.");

            LearningHistory = LearningHistory.Factory.Completed(LearningHistory.TotalLessons, LearningHistory.OverallProgress);
        }

        protected void Validate()
        {
            Validations.ValidateIfEmpty(CourseId, "Course ID is required.");
            Validations.ValidateIfEmpty(CourseName, "Course name is required.");
            Validations.ValidateIfLessOrEqual(Value, 0, "Course value must be greater than zero.");
        }
    }
}
