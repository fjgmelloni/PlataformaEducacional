using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment
{
    public class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public CompleteEnrollmentCommandHandler(IStudentRepository studentRepository, IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(CompleteEnrollmentCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
                return false;

            var enrollment = await _studentRepository.GetEnrollmentWithStudentById(message.EnrollmentId, cancellationToken);

            if (enrollment is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("enrollment", "Enrollment not found!"));
                return false;
            }

            enrollment.Activate();

            return await _studentRepository.UnitOfWork.Commit();
        }

        private bool ValidateCommand(Command message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(
                    new DomainNotification(message.MessageType, error.ErrorMessage)
                );
            }

            return false;
        }
    }
}
