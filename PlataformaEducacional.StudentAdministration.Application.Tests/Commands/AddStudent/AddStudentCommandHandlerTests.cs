using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.AddStudent
{
    public class AddStudentCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly AddStudentCommandHandler _handler;

        public AddStudentCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<AddStudentCommandHandler>();
        }

        [Fact(DisplayName = "Should return false when command is invalid")]
        [Trait("Category", "AddStudentCommandHandler")]
        public async Task AddStudent_ShouldReturnFalse_WhenCommandInvalid()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.Empty, "");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.Exactly(2));
        }

        [Fact(DisplayName = "Should add student successfully when command is valid")]
        [Trait("Category", "AddStudentCommandHandler")]
        public async Task AddStudentCommandValid_ShouldExecuteSuccessfully()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.NewGuid(), "Rinaldo");

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);

            _mocker.GetMock<IStudentRepository>()
                .Verify(r => r.InsertAsync(It.IsAny<Student>(), CancellationToken.None), Times.Once);

            _mocker.GetMock<IStudentRepository>()
                .Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }
    }
}
