using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PerformLesson
{
    public sealed class PerformLessonCommandHandler : IRequestHandler<PerformLessonCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PerformLessonCommandHandler(
            IStudentRepository studentRepository,
            IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(PerformLessonCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var enrollment = await _studentRepository.GetEnrollmentWithProgressById(command.EnrollmentId, cancellationToken);
            if (enrollment is null)
            {
                await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("Lesson", "Matrícula não encontrada.")
                );
                return false;
            }

            var progress = new LessonProgress(command.LessonId);
            enrollment.RecordLesson(progress);
            await _studentRepository.AddLessonProgressAsync(progress, cancellationToken);
            return await _studentRepository.UnitOfWork.Commit();
        }

        private bool ValidateCommand(Command command)
        {
            if (command.IsValid()) return true;

            foreach (var error in command.ValidationResult.Errors)
                _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification(command.MessageType, error)
                );

            return false;
        }
    }
}
