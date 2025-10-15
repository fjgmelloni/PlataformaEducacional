using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.UpdateCourse
{
    public sealed class UpdateCourseCommandValidator
    {
        public ValidationResult Validate(UpdateCourseCommand command)
        {
            var result = new ValidationResult();

            if (command.CourseId == Guid.Empty)
                result.AddError("Course ID is required.");

            if (string.IsNullOrWhiteSpace(command.Name))
                result.AddError("Course name is required.");
            else if (command.Name.Length > 255)
                result.AddError("Course name must be at most 255 characters.");

            if (string.IsNullOrWhiteSpace(command.SyllabusDescription))
                result.AddError("Syllabus description is required.");
            else if (command.SyllabusDescription.Length > 1000)
                result.AddError("Syllabus description must be at most 1000 characters.");

            if (command.SyllabusWorkload <= 0)
                result.AddError("Course workload must be greater than 0.");

            if (command.Price <= 0)
                result.AddError("Course price must be greater than 0.");

            return result;
        }
    }
}
