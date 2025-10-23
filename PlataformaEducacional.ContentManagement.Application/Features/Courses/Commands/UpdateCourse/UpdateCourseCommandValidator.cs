using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.UpdateCourse
{
    public sealed class UpdateCourseCommandValidator
    {
        public ValidationResultCustom Validate(UpdateCourseCommand command)
        {
            var result = new ValidationResultCustom();

            if (command.CourseId == Guid.Empty)
                result.AddError("O identificador do curso é obrigatório.");

            if (string.IsNullOrWhiteSpace(command.Name))
                result.AddError("O nome do curso é obrigatório.");
            else if (command.Name.Length > 255)
                result.AddError("O nome do curso deve ter no máximo 255 caracteres.");

            if (string.IsNullOrWhiteSpace(command.SyllabusDescription))
                result.AddError("A descrição do curso é obrigatória.");
            else if (command.SyllabusDescription.Length > 1000)
                result.AddError("A descrição do curso deve ter no máximo 1000 caracteres.");

            if (command.SyllabusWorkload <= 0)
                result.AddError("A carga horária deve ser maior que 0.");

            if (command.Price <= 0)
                result.AddError("O preço do curso deve ser maior que 0.");

            return result;
        }
    }
}
