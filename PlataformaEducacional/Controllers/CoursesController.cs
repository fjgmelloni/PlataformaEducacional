using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacional.Api.Requests.Course;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddCourse;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddLesson;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.UpdateCourse;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries.ViewModels;
using System.Net;

namespace PlataformaEducacional.Api.Controllers
{
    [Route("api/courses")]
    public class CoursesController : MainController
    {
        private readonly ICourseQueries _courseQueries;
        private readonly IStudentQueries _studentQueries;
        private readonly IMediatorHandler _mediatorHandler;

        public CoursesController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            ICurrentUser identityUser,
            ICourseQueries courseQueries,
            IStudentQueries studentQueries
        ) : base(notifications, mediatorHandler, identityUser)
        {
            _courseQueries = courseQueries;
            _studentQueries = studentQueries;
            _mediatorHandler = mediatorHandler;
        }

        [AllowAnonymous]
        [HttpGet("available-courses")]
        public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetAvailableCourses(CancellationToken cancellationToken)
        {
            var courses = await _courseQueries.GetAvailableWithLessonsAsync(cancellationToken);
            return CustomResponse(HttpStatusCode.OK, courses);
        }

   [HttpGet("{courseId:guid}")]
[AllowAnonymous]
public async Task<ActionResult<CourseViewModel>> GetCourseDetails(Guid courseId, CancellationToken cancellationToken)
{
    var course = await _courseQueries.GetWithLessonsByIdAsync(courseId, cancellationToken);
    return CustomResponse(HttpStatusCode.OK, course);
}

        [HttpGet("all-courses")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetAllCourses(CancellationToken cancellationToken)
        {
            var courses = await _courseQueries.GetAllAsync(cancellationToken);
            return CustomResponse(HttpStatusCode.OK, courses);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new AddCourseCommand(
                request.Name,
                request.ContentDescription,
                request.Workload,
                request.Price,
                request.Available
            );

            await _mediatorHandler.SendCommandAsync(command);
            return CustomResponse(HttpStatusCode.Created);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                NotifyError("Course", "O id informado não é o mesmo que foi passado no body.");
                return CustomResponse();
            }

            var command = new UpdateCourseCommand(
                request.Id,
                request.Name,
                request.ContentDescription,
                request.Workload,
                request.Price,
                request.Available
            );

            await _mediatorHandler.SendCommandAsync(command);
            return CustomResponse();
        }

        [HttpPost("lesson")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddLesson([FromBody] AddLessonRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new AddLessonCommand(
                request.Title,
                request.Content,
                request.Order,
                request.Material,
                request.CourseId
            );

            await _mediatorHandler.SendCommandAsync(command);
            return CustomResponse(HttpStatusCode.Created);
        }

        [HttpGet("{courseId:guid}/active-students")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetActiveStudentsByCourse(Guid courseId, CancellationToken cancellationToken)
        {
            var students = await _studentQueries.GetEnrolledStudentsByCourseId(courseId, cancellationToken);
            return CustomResponse(HttpStatusCode.OK, students);
        }

        [HttpGet("{courseId:guid}/pending-students")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetPendingStudentsByCourse(Guid courseId, CancellationToken cancellationToken)
        {
            var students = await _studentQueries.GetPendingStudentsByCourseId(courseId, cancellationToken);
            return CustomResponse(HttpStatusCode.OK, students);
        }
    }
}
