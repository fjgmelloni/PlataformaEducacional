using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.AddLesson
{
    public sealed class AddLessonCommandValidator
    {
        public ValidationResult Validate(AddLessonCommand command)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(command.Title))
                result.AddError("Lesson title is required.");
            else if (command.Title.Length > 255)
                result.AddError("Lesson title must be at most 255 characters.");

            if (string.IsNullOrWhiteSpace(command.Content))
                result.AddError("Lesson content is required.");
            else if (command.Content.Length > 1000)
                result.AddError("Lesson content must be at most 1000 characters.");

            if (command.Order <= 0)
                result.AddError("Lesson order must be greater than 0.");

            if (command.CourseId == Guid.Empty)
                result.AddError("Course ID is required.");

            return result;
        }
    }
}
