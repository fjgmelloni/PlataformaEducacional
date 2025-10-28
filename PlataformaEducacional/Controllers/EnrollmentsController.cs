using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PerformLesson;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PaymentEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;
using System.Net;
using PlataformaEducacional.Api.Extensions;
using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.Api.Controllers
{
    [Route("api/enrollments")]
    public sealed class EnrollmentsController : MainController
    {
        private readonly ICourseQueries _courseQueries;
        private readonly IStudentQueries _studentQueries;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICurrentUser _identityUser;

        public EnrollmentsController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            ICurrentUser identityUser,
            ICourseQueries courseQueries,
            IStudentQueries studentQueries
        ) : base(notifications, mediatorHandler, identityUser)
        {
            _courseQueries = courseQueries;
            _mediatorHandler = mediatorHandler;
            _studentQueries = studentQueries;
            _identityUser = identityUser;
        }

        // === Processar pagamento da matrícula ===
        [HttpPost("{enrollmentId:guid}/payment")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> ProcessPayment(Guid enrollmentId, [FromBody] ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new PaymentEnrollmentCommand(
                enrollmentId,
                Guid.Parse(_identityUser.GetUserId()),
                request.Total,
                request.CardName,
                request.CardNumber,
                request.CardExpiration,
                request.CardCvv
            );

            await _mediatorHandler.SendCommandAsync(command);
            return CustomResponse();
        }

        // === Visualizar aula ===
        [HttpGet("{enrollmentId:guid}/lesson/{lessonId:guid}")]
        [Authorize(Roles = "STUDENT")]
        public async Task<ActionResult<LessonViewModel>> ViewLesson(Guid enrollmentId, Guid lessonId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Enrollment", "Enrollment not found.");
                return CustomResponse();
            }

            if (enrollment.EnrollmentStatus != EnrollmentStatus.Active)
            {
                NotifyError("Enrollment", "Pending payment.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Enrollment", "Enrollment does not belong to this student.");
                return CustomResponse();
            }

            var lesson = await _courseQueries.GetLessonByCourseAndIdAsync(enrollment.CourseId, lessonId, cancellationToken);
            if (lesson is null)
            {
                NotifyError("Enrollment", "Lesson does not belong to the course.");
                return CustomResponse();
            }

            return CustomResponse(HttpStatusCode.OK, lesson);
        }

        // === Concluir aula ===
        [HttpPut("{enrollmentId:guid}/complete-lesson/{lessonId:guid}")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> CompleteLesson(Guid enrollmentId, Guid lessonId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Enrollment", "Enrollment not found.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Enrollment", "Enrollment does not belong to this student.");
                return CustomResponse();
            }

            var lesson = await _courseQueries.GetLessonByCourseAndIdAsync(enrollment.CourseId, lessonId, cancellationToken);
            if (lesson is null)
            {
                NotifyError("Enrollment", "Lesson does not belong to the course.");
                return CustomResponse();
            }

            var command = new PerformLessonCommand(enrollmentId, lessonId);
            await _mediatorHandler.SendCommandAsync(command);

            return CustomResponse();
        }

        [HttpPut("{enrollmentId:guid}/complete-course")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> CompleteCourse(Guid enrollmentId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Matrícula", "Matrícula não encontrada.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Matrícula", "Matrícula não pertence a este aluno.");
                return CustomResponse();
            }

            var command = new CompleteCourseCommand(
                enrollmentId,
                Guid.Parse(_identityUser.GetUserId())
            );

            await _mediatorHandler.SendCommandAsync(command);
            return CustomResponse();
        }


        // === Obter certificado ===
        [HttpGet("{enrollmentId:guid}/certificate")]
        [Authorize(Roles = "STUDENT")]
        public async Task<ActionResult<LessonViewModel>> GetCertificate(Guid enrollmentId, CancellationToken cancellationToken)
        {
            var enrollment = await _studentQueries.GetEnrollment(enrollmentId, cancellationToken);
            if (enrollment is null)
            {
                NotifyError("Enrollment", "Enrollment not found.");
                return CustomResponse();
            }

            if (enrollment.EnrollmentStatus != EnrollmentStatus.Active)
            {
                NotifyError("Enrollment", "Pending payment.");
                return CustomResponse();
            }

            if (enrollment.StudentId != Guid.Parse(_identityUser.GetUserId()))
            {
                NotifyError("Enrollment", "Enrollment does not belong to this student.");
                return CustomResponse();
            }

            if (enrollment.CourseStatus != CourseStatus.Completed)
            {
                NotifyError("Enrollment", "The student has not yet completed the course.");
                return CustomResponse();
            }

            var certificate = await _studentQueries.GetCertificate(enrollmentId, cancellationToken);
            return CustomResponse(HttpStatusCode.OK, certificate);
        }
    }
}
