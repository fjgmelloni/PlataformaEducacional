using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment
{
    public sealed class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public CancelEnrollmentCommandHandler(
            IStudentRepository studentRepository,
            IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(CancelEnrollmentCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var enrollment = await _studentRepository.GetEnrollmentWithStudentById(command.EnrollmentId, cancellationToken);

            if (enrollment is null)
            {
                await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("Enrollment", "Enrollment not found.")
                );
                return false;
            }

            enrollment.Deactivate();
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
