using PlataformaEducacional.Core.Domain;        

namespace PlataformaEducacional.ContentManagement.Domain.Lessons
{
    public sealed class Lesson : Entity
    {
        public string Title { get; private set; } = null!;
        public string Content { get; private set; } = null!;
        public int Order { get; private set; }
        public string? Material { get; private set; }

        public Guid CourseId { get; private set; }
        public Courses.Course Course { get; private set; } = null!;

        protected Lesson() { } // EF

        public Lesson(string title, string content, int order, string? material)
        {
            Title = title;
            Content = content;
            Order = order;
            Material = material;

            Validate();
        }

        public void LinkCourse(Guid courseId)
        {
            Guard.AgainstEmpty(courseId, "Course cannot be empty.");
            CourseId = courseId;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
            Validate();
        }

        public void UpdateContent(string content)
        {
            Content = content;
            Validate();
        }

        public void UpdateOrder(int order)
        {
            Order = order;
            Validate();
        }

        private void Validate()
        {
            Guard.AgainstNullOrWhiteSpace(Title, "The lesson title is required.");
            Guard.AgainstNullOrWhiteSpace(Content, "The lesson content is required.");
            Guard.AgainstLessOrEqual(Order, 0, "The lesson order must be greater than 0.");

            if (Title.Length > 200)
                throw new DomainException("The lesson title must be at most 200 characters.");


        }
    }
}
