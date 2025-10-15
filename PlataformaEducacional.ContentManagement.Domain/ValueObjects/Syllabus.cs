using PlataformaEducacional.Core.Domain; 
namespace PlataformaEducacional.ContentManagement.Domain.ValueObjects
{
    public sealed class Syllabus
    {
        public string Description { get; private set; } = null!;
        public int Workload { get; private set; } 

        private Syllabus() { }

        public Syllabus(string description, int workload)
        {
            Description = description;
            Workload = workload;
            Validate();
        }

        private void Validate()
        {
            Guard.AgainstNullOrWhiteSpace(Description, "The syllabus description is required.");
            Guard.AgainstLessOrEqual(Workload, 0, "The course workload must be greater than 0.");

            if (Description.Length > 1000)
                throw new DomainException("The syllabus description must be at most 1000 characters.");
        }
    }
}
