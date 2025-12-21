using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Data;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.GenerateCertificate
{
    public class GenerateCertificateCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly GenerateCertificateCommandHandler _handler;

        public GenerateCertificateCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<GenerateCertificateCommandHandler>();
        }

        [Fact(DisplayName = "Should generate certificate and commit when valid")]
        public async Task Should_Generate_Certificate_And_Commit()
        {
            var enrollmentId = Guid.NewGuid();
            var enrollment = new Enrollment(enrollmentId, "C# Course", 1, 300);

            enrollment.AssignStudent(Guid.NewGuid());
            enrollment.LinkStudent(new Student(enrollment.StudentId, "Felicio"));
            enrollment.Activate();

            var lesson = new LessonProgress(Guid.NewGuid());
            lesson.AssignEnrollment(enrollment.Id);
            enrollment.RecordLesson(lesson);
            enrollment.CompleteCourse();

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithCertificateById(enrollmentId, default))
                .ReturnsAsync(enrollment);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GenerateCertificateAsync(
                    It.IsAny<Certificate>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mocker.GetMock<IStudentRepository>()
                .SetupGet(r => r.UnitOfWork)
                .Returns(_mocker.GetMock<IUnitOfWork>().Object);

            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.Commit())
                .ReturnsAsync(true);

            var command = new GenerateCertificateCommand(enrollmentId);

            var result = await _handler.Handle(command, default);

            Assert.True(result);

            _mocker.GetMock<IStudentRepository>().Verify(
                r => r.GenerateCertificateAsync(
                    It.IsAny<Certificate>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }




        [Fact(DisplayName = "Should notify and return false when enrollment not found")]
        public async Task Should_Return_False_When_Enrollment_Not_Found()
        {
            var command = new GenerateCertificateCommand(Guid.NewGuid());

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithCertificateById(It.IsAny<Guid>(), default))
                .ReturnsAsync((Enrollment)null!);

            var result = await _handler.Handle(command, default);

            Assert.False(result);

            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.Is<DomainNotification>(n => n.Value == "Matrícula não encontrada.")),
                Times.Once);
        }

        [Fact(DisplayName = "Should notify and return false when certificate already exists")]
        public async Task Should_Return_False_When_Certificate_Already_Exists()
        {
            var enrollmentId = Guid.NewGuid();
            var enrollment = new Enrollment(Guid.NewGuid(), "C# Course", 10, 300);
            enrollment.AssignStudent(Guid.NewGuid());
            enrollment.AddCertificate(new Certificate(enrollmentId));

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithCertificateById(enrollmentId, default))
                .ReturnsAsync(enrollment);

            var command = new GenerateCertificateCommand(enrollmentId);

            var result = await _handler.Handle(command, default);

            Assert.False(result);

            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.Is<DomainNotification>(n => n.Value == "Certificado já foi gerado.")),
                Times.Once);
        }
    }
}
