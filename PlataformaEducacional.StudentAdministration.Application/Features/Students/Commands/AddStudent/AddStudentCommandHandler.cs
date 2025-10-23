using MediatR;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.Base;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent
{
    public sealed class AddStudentCommandHandler : IRequestHandler<AddStudentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public AddStudentCommandHandler(
            IStudentRepository studentRepository,
            IMediatorHandler mediatorHandler)
        {
            _studentRepository = studentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddStudentCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var student = new Student(command.UserId, command.Name);

            await _studentRepository.InsertAsync(student, cancellationToken);
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
