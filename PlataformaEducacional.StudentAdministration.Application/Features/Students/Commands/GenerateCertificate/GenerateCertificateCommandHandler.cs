using MediatR;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate
{
    public class GenerateCertificateCommandHandler : IRequestHandler<GenerateCertificateCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;

        public GenerateCertificateCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<bool> Handle(GenerateCertificateCommand message, CancellationToken cancellationToken)
        {
            var certificate = new Certificate(message.EnrollmentId);

            await _studentRepository.GenerateCertificateAsync(certificate, cancellationToken);
            return await _studentRepository.UnitOfWork.Commit();
        }
    }
}
