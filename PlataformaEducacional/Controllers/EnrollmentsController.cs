using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.DomainObjects;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Commands.CompleteCourse;
using PlataformaEducacional.StudentAdministration.Application.Commands.ProcessPayment;
using PlataformaEducacional.StudentAdministration.Application.Commands.PerformLesson;
using PlataformaEducacional.StudentAdministration.Application.Queries;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.ContentManagement.Application.Queries;
using PlataformaEducacional.ContentManagement.Application.Queries.ViewModels;
using System.Net;

namespace PlataformaEducacional.Api.Controllers
{
    [Route("api/enrollments")]
    public class EnrollmentsController : MainController
    {
        private readonly ICourseQueries _courseQueries;
        private readonly IStudentQueries _studentQueries;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAppIdentityUser _identityUser;

        public EnrollmentsController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            IAppIdentityUser identityUser,
            ICourseQueries courseQueries,
            IStudentQueries studentQueries
        ) : base(notifications, mediatorHandler, identityUser)
        {
            _courseQueries = courseQueries;
            _mediatorHandler = mediatorHandler;
            _studentQueries = studentQueries;
            _identityUser = identityUser;
        }

        [HttpPost("{enrollmentId:guid}/payment")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> ProcessPayment(Guid enrollmentId, [FromBody] ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new ProcessPaymentCommand(
                enrollmentId,
                Guid.Parse(_identityUser.GetUserId()),
                request.Total,
                request.CardName,
                request.CardNumber,
                request.CardExpiration,
                request.CardCvv
            );

            await _mediatorHandler.SendCommand(command);
            return CustomResponse();
        }

        [HttpGet("{enrollmentId:guid}/lesson/{lessonId:guid}")]
        [Authorize(Roles = "STUDENT")]
        public async Task<ActionResult<LessonViewModel>> ViewLesson(Guid enrollmentId, Guid lessonId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Student", "Matrícula não encontrada.");
                return CustomResponse();
            }

            if (enrollment.EnrollmentStatus != EnrollmentStatus.Active)
            {
                NotifyError("Student", "Pagamento pendente.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Student", "Matrícula não pertence ao aluno.");
                return CustomResponse();
            }

            var lesson = await _courseQueries.GetLessonByCourseAndLessonId(enrollment.CourseId, lessonId, cancellationToken);
            if (lesson is null)
            {
                NotifyError("Student", "Aula não pertence ao curso.");
                return CustomResponse();
            }

            return CustomResponse(HttpStatusCode.OK, lesson);
        }

        [HttpPut("{enrollmentId:guid}/complete-lesson/{lessonId:guid}")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> CompleteLesson(Guid enrollmentId, Guid lessonId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Student", "Matrícula não encontrada.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Student", "Matrícula não pertence ao aluno.");
                return CustomResponse();
            }

            var lesson = await _courseQueries.GetLessonByCourseAndLessonId(enrollment.CourseId, lessonId, cancellationToken);
            if (lesson is null)
            {
                NotifyError("Student", "Aula não pertence ao curso.");
                return CustomResponse();
            }

            var command = new PerformLessonCommand(enrollmentId, lessonId);
            await _mediatorHandler.SendCommand(command);

            return CustomResponse();
        }

        [HttpPut("{enrollmentId:guid}/complete-course")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> CompleteCourse(Guid enrollmentId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Student", "Matrícula não encontrada.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Student", "Matrícula não pertence ao aluno.");
                return CustomResponse();
            }

            var command = new CompleteCourseCommand(enrollmentId);
            await _mediatorHandler.SendCommand(command);

            return CustomResponse();
        }

        [HttpGet("{enrollmentId:guid}/certificate")]
        [Authorize(Roles = "STUDENT")]
        public async Task<ActionResult<LessonViewModel>> GetCertificate(Guid enrollmentId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Student", "Matrícula não encontrada.");
                return CustomResponse();
            }

            if (enrollment.EnrollmentStatus != EnrollmentStatus.Active)
            {
                NotifyError("Student", "Pagamento pendente.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Student", "Matrícula não pertence ao aluno.");
                return CustomResponse();
            }

            if (enrollment.CourseStatus != CourseStatus.Completed)
            {
                NotifyError("Student", "Aluno ainda não finalizou o curso.");
                return CustomResponse();
            }

            var certificate = await _studentQueries.GetCertificate(enrollmentId, cancellationToken);
            return CustomResponse(HttpStatusCode.OK, certificate);
        }
    }
}
