using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Events;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse
{
    public class CompleteCourseCommandHandler : IRequestHandler<CompleteCourseCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public CompleteCourseCommandHandler(IStudentRepository studentRepository, IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(CompleteCourseCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
                return false;

            var enrollment = await _studentRepository.GetEnrollmentWithProgressById(message.EnrollmentId, cancellationToken);

            if (enrollment is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(nameof(EnrollInCourseCommand), "Enrollment not found!"));
                return false;
            }

            if (!enrollment.IsActive())
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(nameof(EnrollInCourseCommand), "Enrollment pending payment!"));
                return false;
            }

            if (enrollment.LearningHistory.OverallProgress < 100)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(nameof(EnrollInCourseCommand), "There are still lessons to complete."));
                return false;
            }

            if (enrollment.LearningHistory.CourseStatus == CourseStatus.Completed)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(nameof(EnrollInCourseCommand), "Course already completed."));
                return false;
            }

            enrollment.CompleteCourse();
            enrollment.AddEvent(new CourseCompletedEvent(enrollment.Id));

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
