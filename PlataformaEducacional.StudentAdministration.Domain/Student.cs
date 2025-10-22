using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.StudentAdministration.Domain
{
    public class Student : Entity, IAggregateRoot
    {
        public string Name { get; private set; } = null!;

        private readonly List<Enrollment> _enrollments;
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments;

        protected Student()
        {
            _enrollments = new List<Enrollment>();
        }

        public Student(Guid studentId, string name)
        {
            Id = studentId;
            Name = name;
            _enrollments = new List<Enrollment>();
            Validate();
        }

        public bool HasEnrollment(Enrollment enrollment)
        {
            return _enrollments.Any(e => e.CourseId == enrollment.CourseId);
        }

        public void EnrollInCourse(Enrollment enrollment)
        {
            if (HasEnrollment(enrollment))
                throw new DomainException("Student already enrolled in this course.");

            enrollment.AssignStudent(Id);
            _enrollments.Add(enrollment);
        }

        protected void Validate()
        {
            Validations.ValidateIfEmpty(Id, "The student ID is required.");
            Validations.ValidateIfEmpty(Name, "The student name is required.");
        }
    }
}
