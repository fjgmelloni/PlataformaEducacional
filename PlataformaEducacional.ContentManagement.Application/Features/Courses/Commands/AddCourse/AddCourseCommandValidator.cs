// Remova completamente o using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.AddCourse
{
    public sealed class AddCourseCommandValidator
    {
        public ValidationResult Validate(AddCourseCommand command)
        {
            var result = new ValidationResult();

            // Name
            if (string.IsNullOrWhiteSpace(command.Name))
                result.AddError("Course name is required.");
            else if (command.Name.Length > 255)
                result.AddError("Course name must be at most 255 characters.");

            // SyllabusDescription
            if (string.IsNullOrWhiteSpace(command.SyllabusDescription))
                result.AddError("Syllabus description is required.");
            else if (command.SyllabusDescription.Length > 1000)
                result.AddError("Syllabus description must be at most 1000 characters.");

            // SyllabusWorkload
            if (command.SyllabusWorkload <= 0)
                result.AddError("Course workload must be greater than 0.");

            // Price
            if (command.Price <= 0)
                result.AddError("Course price must be greater than 0.");

            return result;
        }
    }
}
