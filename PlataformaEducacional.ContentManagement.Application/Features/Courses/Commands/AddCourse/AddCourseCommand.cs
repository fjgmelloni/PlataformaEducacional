using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.AddCourse
{
    public sealed class AddCourseCommand : Command
    {
        public AddCourseCommand(string name, string syllabusDescription, int syllabusWorkload, decimal price, bool isAvailable)
        {
            Name = name;
            SyllabusDescription = syllabusDescription;
            SyllabusWorkload = syllabusWorkload;
            Price = price;
            IsAvailable = isAvailable;
        }

        public string Name { get; }
        public string SyllabusDescription { get; }
        public int SyllabusWorkload { get; }
        public decimal Price { get; }
        public bool IsAvailable { get; }

        public override bool IsValid()
        {
            // Aqui você implementa a validação simples (sem FluentValidation)
            if (string.IsNullOrWhiteSpace(Name)) return false;
            if (string.IsNullOrWhiteSpace(SyllabusDescription)) return false;
            if (SyllabusWorkload <= 0) return false;
            if (Price < 0) return false;

            return true;
        }
    }
}
