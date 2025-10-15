using PlataformaEducacional.Core.Domain;         
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.ContentManagement.Domain.ValueObjects;

namespace PlataformaEducacional.ContentManagement.Domain.Courses
{
    public sealed class Course : Entity, IAggregateRoot
    {
        public string Name { get; private set; } = null!;
        public Syllabus Syllabus { get; private set; } = null!;
        public decimal Price { get; private set; }
        public bool IsAvailable { get; private set; }

        private readonly List<Lesson> _lessons;
        public IReadOnlyCollection<Lesson> Lessons => _lessons;

        protected Course() { _lessons = new List<Lesson>(); }

        public Course(string name, Syllabus syllabus, decimal price, bool isAvailable)
        {
            Name = name;
            Syllabus = syllabus;
            Price = price;
            IsAvailable = isAvailable;
            _lessons = new List<Lesson>();

            Validate();
        }

        public void UpdateName(string name)
        {
            Name = name;
            Validate();
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
            Validate();
        }

        public void MakeAvailable() => IsAvailable = true;
        public void MakeUnavailable() => IsAvailable = false;

        public void UpdateSyllabus(Syllabus syllabus)
        {
            if (syllabus is null)
                throw new DomainException("The syllabus is required.");

            Syllabus = syllabus;
        }

        public void AddLesson(Lesson lesson)
        {
            if (lesson is null)
                throw new DomainException("Lesson is required.");

            if (LessonExists(lesson))
                throw new DomainException("Lesson already associated with this course.");

            lesson.LinkCourse(Id);
            _lessons.Add(lesson);
        }

        public bool LessonExists(Lesson lesson)
            => _lessons.Any(a => string.Equals(a.Title, lesson.Title, StringComparison.OrdinalIgnoreCase));

        private void Validate()
        {
            Guard.AgainstNullOrWhiteSpace(Name, "The course name is required.");
            Guard.AgainstLessOrEqual(Price, 0, "The course price must be greater than 0.");

            if (Name.Length > 255)
                throw new DomainException("The course name must be at most 255 characters.");

            if (Syllabus is null)
                throw new DomainException("The syllabus is required.");
        }
    }
}
