using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate
{
    public class GenerateCertificateCommand : Command
    {
        public GenerateCertificateCommand(Guid enrollmentId)
        {
            EnrollmentId = enrollmentId;
        }

        public Guid EnrollmentId { get; private set; }
    }
}
