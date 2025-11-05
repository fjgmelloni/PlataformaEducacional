using Moq;
using Moq.AutoMock;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries;
using PlataformaEducacional.StudentAdministration.Application.Queries;
using PlataformaEducacional.StudentAdministration.Application.Queries.ViewModels;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Queries
{
    public class StudentQueriesTests
    {
        private readonly AutoMocker _mocker;
        private readonly StudentQueries _queries;

        private readonly Guid _studentId = Guid.NewGuid();
        private readonly Guid _courseId = Guid.NewGuid();
        private readonly Guid _enrollmentId = Guid.NewGuid();

        public StudentQueriesTests()
        {
            _mocker = new AutoMocker();
            _queries = _mocker.CreateInstance<StudentQueries>();
        }

        [Fact(DisplayName = "Should call repository and return pending enrollment ViewModels")]
        [Trait("Category", "Student Administration - StudentQueries")]
        public async Task GetPendingPaymentEnrollmentsByStudentId_WhenDataExists_ShouldReturnEnrollmentViewModels()
        {
            // Arrange
            var enrollmentViewModels = new List<EnrollmentViewModel>
            {
                new EnrollmentViewModel(
                    _enrollmentId,
                    _studentId,
                    "Felíco",
                    _courseId,
                    "DevExpert",
                    EnrollmentStatus.Active,
                    DateTime.Now,
                    CourseStatus.NotStarted,
                    null,
                    0
                )
            };

            var student = new Student(_studentId, "Felíco");
            var enrollment = new Enrollment(_courseId, "DevExpert", 5, 500);

            enrollment.AssignStudent(_studentId);

            var domainEnrollments = new List<Enrollment> { enrollment };

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetPendingPaymentEnrollmentsByStudentId(_studentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(domainEnrollments);

            // Act
            var result = await _queries.GetPendingPaymentEnrollmentsByStudentId(_studentId, CancellationToken.None);

            // Assert
            _mocker.GetMock<IStudentRepository>().Verify(
                r => r.GetPendingPaymentEnrollmentsByStudentId(_studentId, It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.NotEmpty(result);
            Assert.Equal(enrollmentViewModels.Count, result.Count());
        }
    }
}
