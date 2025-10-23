using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddLesson
{
    public sealed class AddLessonCommandValidator
    {
        public ValidationResultCustom Validate(AddLessonCommand command)
        {
            var result = new ValidationResultCustom();

            if (string.IsNullOrWhiteSpace(command.Title))
                result.AddError("O título da aula é obrigatório.");
            else if (command.Title.Length > 255)
                result.AddError("O título da aula deve ter no máximo 255 caracteres.");

            if (string.IsNullOrWhiteSpace(command.Content))
                result.AddError("O conteúdo da aula é obrigatório.");
            else if (command.Content.Length > 1000)
                result.AddError("O conteúdo deve ter no máximo 1000 caracteres.");

            if (command.Order <= 0)
                result.AddError("A ordem da aula deve ser maior que 0.");

            if (command.CourseId == Guid.Empty)
                result.AddError("O identificador do curso é obrigatório.");

            return result;
        }
    }
}
