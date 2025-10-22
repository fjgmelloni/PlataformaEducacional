using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.StudentAdministration.Domain
{
    public class LearningHistory
    {
        public int TotalLessons { get; private set; }
        public double OverallProgress { get; private set; }
        public CourseStatus CourseStatus { get; private set; }
        public DateTime? CompletionDate { get; private set; }

        protected LearningHistory() { }

        public static class Factory
        {
            public static LearningHistory PendingPayment(int totalLessons)
            {
                var history = new LearningHistory
                {
                    TotalLessons = totalLessons,
                    OverallProgress = 0,
                    CourseStatus = CourseStatus.NotStarted,
                    CompletionDate = null
                };

                history.Validate();
                return history;
            }

            public static LearningHistory InProgress(int totalLessons, double overallProgress)
            {
                var history = new LearningHistory
                {
                    TotalLessons = totalLessons,
                    OverallProgress = overallProgress,
                    CourseStatus = CourseStatus.InProgress,
                    CompletionDate = null
                };

                history.Validate();
                return history;
            }

            public static LearningHistory Completed(int totalLessons, double overallProgress)
            {
                var history = new LearningHistory
                {
                    TotalLessons = totalLessons,
                    OverallProgress = overallProgress,
                    CourseStatus = CourseStatus.Completed,
                    CompletionDate = DateTime.Now
                };

                history.Validate();
                return history;
            }
        }

        protected void Validate()
        {
            Validations.ValidateIfLessOrEqual(TotalLessons, 0, "The course must have more than 0 lessons.");
        }
    }
}
