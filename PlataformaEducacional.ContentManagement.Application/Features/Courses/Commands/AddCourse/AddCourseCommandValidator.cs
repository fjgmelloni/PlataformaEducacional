using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddCourse
{
    public sealed class AddCourseCommandValidator
    {
        public ValidationResultCustom Validate(AddCourseCommand command)
        {
            var result = new ValidationResultCustom();

            // Name
            if (string.IsNullOrWhiteSpace(command.Name))
                result.AddError("O nome do curso é obrigatório.");
            else if (command.Name.Length > 255)
                result.AddError("O nome do curso deve ter no máximo 255 caracteres.");

            // SyllabusDescription
            if (string.IsNullOrWhiteSpace(command.SyllabusDescription))
                result.AddError("A descrição do curso é obrigatória.");
            else if (command.SyllabusDescription.Length > 1000)
                result.AddError("A descrição deve ter no máximo 1000 caracteres.");

            // Workload
            if (command.SyllabusWorkload <= 0)
                result.AddError("A carga horária deve ser maior que 0.");

            // Price
            if (command.Price <= 0)
                result.AddError("O preço do curso deve ser maior que 0.");

            return result;
        }
    }
}
