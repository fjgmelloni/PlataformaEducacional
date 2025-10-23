using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment
{
    public sealed class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public CompleteEnrollmentCommandHandler(
            IStudentRepository studentRepository,
            IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(CompleteEnrollmentCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var enrollment = await _studentRepository.GetEnrollmentWithStudentById(command.EnrollmentId, cancellationToken);

            if (enrollment is null)
            {
                await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("Enrollment", "Matrícula não encontrada.")
                );
                return false;
            }

            enrollment.Activate();
            return await _studentRepository.UnitOfWork.Commit();
        }

        private bool ValidateCommand(Command command)
        {
            if (command.IsValid()) return true;

            foreach (var error in command.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification(command.MessageType, error)
                );
            }

            return false;
        }
    }
}
