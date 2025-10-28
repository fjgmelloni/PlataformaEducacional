using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacional.Api.Requests.Enrollment;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;
using System.Net;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;

namespace PlataformaEducacional.Api.Controllers
{
    [Route("api/students")]
    public class StudentsController : MainController
    {
        private readonly ICourseQueries _courseQueries;
        private readonly IStudentQueries _studentQueries;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICurrentUser _identityUser;

        public StudentsController(
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

        [HttpGet("active-courses")]
        [Authorize(Roles = "STUDENT")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetActiveCourses(CancellationToken cancellationToken)
        {
            var courses = await _studentQueries.GetActiveEnrollmentsByStudentId(Guid.Parse(_identityUser.GetUserId()), cancellationToken);
            return CustomResponse(HttpStatusCode.OK, courses);
        }

        [HttpGet("pending-courses")]
        [Authorize(Roles = "STUDENT")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetPendingCourses(CancellationToken cancellationToken)
        {
            var courses = await _studentQueries.GetPendingPaymentEnrollmentsByStudentId(Guid.Parse(_identityUser.GetUserId()), cancellationToken);
            return CustomResponse(HttpStatusCode.OK, courses);
        }

        [HttpPost("enroll")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> Enroll([FromBody] EnrollRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var course = await _courseQueries.GetWithLessonsByIdAsync(request.CourseId, cancellationToken);
            if (course is null)
            {
                NotifyError("Student", "Curso não encontrado.");
                return CustomResponse();
            }

            if (!course.IsAvailable)
            {
                NotifyError("Student", "Curso não disponível para matrícula.");
                return CustomResponse();
            }

            var command = new EnrollInCourseCommand(
                course.Id,
                Guid.Parse(_identityUser.GetUserId()),
                course.Name,
                course.Lessons.Count(),
                course.Price
            );

            await _mediatorHandler.SendCommandAsync(command);
            return CustomResponse(HttpStatusCode.Created);
        }

    }
}
