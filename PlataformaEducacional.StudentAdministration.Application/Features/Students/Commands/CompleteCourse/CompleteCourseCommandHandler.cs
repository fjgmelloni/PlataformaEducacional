using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse
{
    public sealed class CompleteCourseCommandHandler : IRequestHandler<CompleteCourseCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public CompleteCourseCommandHandler(
            IStudentRepository studentRepository,
            IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(CompleteCourseCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var enrollment = await _studentRepository.GetEnrollmentWithProgressById(command.EnrollmentId, cancellationToken);

            if (enrollment is null)
            {
                await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("Enrollment", "Matrícula não encontrada.")
                );
                return false;
            }

            try
            {
                enrollment.CompleteCourse();
                return await _studentRepository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("CompleteCourse", ex.Message)
                );
                return false;
            }
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
