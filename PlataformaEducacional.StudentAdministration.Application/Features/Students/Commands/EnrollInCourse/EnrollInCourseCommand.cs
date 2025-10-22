using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse
{
    public class EnrollInCourseCommandHandler : IRequestHandler<EnrollInCourseCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public EnrollInCourseCommandHandler(
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(EnrollInCourseCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
                return false;

            var student = await _studentRepository.GetByIdAsync(message.StudentId, cancellationToken);
            if (student is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("student", "Student not found!"));
                return false;
            }

            var course = await _courseRepository.GetByIdAsync(message.CourseId, cancellationToken);
            if (course is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("course", "Course not found!"));
                return false;
            }

            var enrollment = new Enrollment(message.StudentId, message.CourseId);

            await _studentRepository.EnrollStudentInCourse(enrollment, cancellationToken);

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
