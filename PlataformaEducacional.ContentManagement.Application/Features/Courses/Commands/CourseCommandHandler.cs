using MediatR;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.AddCourse;
using PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.UpdateCourse;
using PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands.AddLesson;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.ContentManagement.Domain.ValueObjects;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands
{
    public sealed class CourseCommandHandler :
        IRequestHandler<AddCourseCommand, bool>,
        IRequestHandler<UpdateCourseCommand, bool>,
        IRequestHandler<AddLessonCommand, bool>
    {
        private readonly ICourseRepository _repo;
        private readonly IMediatorHandler _bus;

        public CourseCommandHandler(ICourseRepository repo, IMediatorHandler bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task<bool> Handle(AddCourseCommand message, CancellationToken ct)
        {
            if (!Validate(message)) return false;

            var existing = await _repo.GetByNameAsync(message.Name, ct);
            if (existing is not null)
            {
                await _bus.PublishNotificationAsync(new DomainNotification("course", "A course with this name already exists."));
                return false;
            }

            var syllabus = new Syllabus(message.SyllabusDescription, message.SyllabusWorkload);
            var course = new Course(message.Name, syllabus, message.Price, message.IsAvailable);

            await _repo.AddAsync(course, ct);
            return await _repo.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(UpdateCourseCommand message, CancellationToken ct)
        {
            if (!Validate(message)) return false;

            var course = await _repo.GetByIdAsync(message.CourseId, ct);
            if (course is null)
            {
                await _bus.PublishNotificationAsync(new DomainNotification("course", "Course not found."));
                return false;
            }

            var byName = await _repo.GetByNameAsync(message.Name, ct);
            if (byName is not null && byName.Id != course.Id)
            {
                await _bus.PublishNotificationAsync(new DomainNotification("course", "Course name already in use."));
                return false;
            }

            course.UpdateName(message.Name);
            course.UpdatePrice(message.Price);
            course.UpdateSyllabus(new Syllabus(message.SyllabusDescription, message.SyllabusWorkload));
            if (message.IsAvailable) course.MakeAvailable(); else course.MakeUnavailable();

            await _repo.UpdateAsync(course, ct);
            return await _repo.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AddLessonCommand message, CancellationToken ct)
        {
            if (!Validate(message)) return false;

            var course = await _repo.GetWithLessonsByIdAsync(message.CourseId, ct);
            if (course is null)
            {
                await _bus.PublishNotificationAsync(new DomainNotification("lesson", "Course not found."));
                return false;
            }

            var lesson = new Lesson(message.Title, message.Content, message.Order, message.Material);
            if (course.LessonExists(lesson))
            {
                await _bus.PublishNotificationAsync(new DomainNotification("lesson", "This course already has a lesson with the same title."));
                return false;
            }

            course.AddLesson(lesson);
            await _repo.AddLessonAsync(lesson, ct);

            return await _repo.UnitOfWork.Commit();
        }

        private bool Validate(Command message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
      
                _ = _bus.PublishNotificationAsync(new DomainNotification(message.MessageType, error));
            }

            return false;
        }
    }
}
