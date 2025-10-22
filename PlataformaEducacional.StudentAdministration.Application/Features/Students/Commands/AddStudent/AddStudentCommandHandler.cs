using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent
{
    public class AddStudentCommandHandler : IRequestHandler<AddStudentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public AddStudentCommandHandler(IStudentRepository studentRepository, IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddStudentCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
                return false;

            var student = new Student(message.UserId, message.Name);

            await _studentRepository.InsertAsync(student, cancellationToken);

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
