using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Events;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Events
{
    public class EnrollmentEventHandler :
        INotificationHandler<EnrollmentPaymentCompletedEvent>,
        INotificationHandler<EnrollmentPaymentDeclinedEvent>,
        INotificationHandler<CourseCompletedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public EnrollmentEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(EnrollmentPaymentCompletedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new CompleteEnrollmentCommand(message.EnrollmentId, message.StudentId));
        }

        public async Task Handle(EnrollmentPaymentDeclinedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new CancelEnrollmentCommand(message.EnrollmentId, message.StudentId));
        }

        public async Task Handle(CourseCompletedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new GenerateCertificateCommand(message.EnrollmentId));
        }
    }
}
