using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse
{
    public sealed class EnrollInCourseCommandHandler : IRequestHandler<EnrollInCourseCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public EnrollInCourseCommandHandler(
            IStudentRepository studentRepository,
            IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(EnrollInCourseCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var student = await _studentRepository.GetWithEnrollmentsById(command.StudentId, cancellationToken);
            if (student is null)
            {
                await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("Enroll", "Aluno não encontrado.")
                );
                return false;
            }

            var enrollment = new Enrollment(command.CourseId, command.CourseName, command.TotalLessons, command.Value);

            try
            {
                student.EnrollInCourse(enrollment);
                await _studentRepository.EnrollStudentInCourse(enrollment, cancellationToken);
                return await _studentRepository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("Enroll", ex.Message)
                );
                return false;
            }
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
