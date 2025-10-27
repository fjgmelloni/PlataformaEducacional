using MediatR;
using PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands;
using PlataformaEducacional.Api.Extensions;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddCourse;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddLesson;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.UpdateCourse;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries;
using PlataformaEducacional.ContentManagement.Data.Repositories;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.FinancialManagement.Core;
using PlataformaEducacional.FinancialManagement.Core.Events;
using PlataformaEducacional.FinancialManagement.Data;
using PlataformaEducacional.FinancialManagement.Data.Repository;
using PlataformaEducacional.FinancialManagement.Integration;
using PlataformaEducacional.StudentAdministration.Application.Events;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PaymentEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PerformLesson;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Events;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Queries;
using PlataformaEducacional.StudentAdministration.Data.Repositories;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;
using Finance = PlataformaEducacional.FinancialManagement.Integration;

namespace PlataformaEducacional.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Mediator
            services.AddMediatR(
               typeof(AddLessonCommand).Assembly,
               typeof(CourseCommandHandler).Assembly
           );

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Content Management
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseQueries, CourseQueries>();

            services.AddScoped<IRequestHandler<AddLessonCommand, bool>, CourseCommandHandler>();
            services.AddScoped<IRequestHandler<AddCourseCommand, bool>, CourseCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCourseCommand, bool>, CourseCommandHandler>();

            // Student Administration
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentQueries, StudentQueries>();

            services.AddScoped<IRequestHandler<AddStudentCommand, bool>, AddStudentCommandHandler>();
            services.AddScoped<IRequestHandler<CompleteEnrollmentCommand, bool>, CompleteEnrollmentCommandHandler>();
            services.AddScoped<IRequestHandler<EnrollInCourseCommand, bool>, EnrollInCourseCommandHandler>();
            services.AddScoped<IRequestHandler<PaymentEnrollmentCommand, bool>, PaymentEnrollmentCommandHandler>();
            services.AddScoped<IRequestHandler<CancelEnrollmentCommand, bool>, CancelEnrollmentCommandHandler>();
            services.AddScoped<IRequestHandler<PerformLessonCommand, bool>, PerformLessonCommandHandler>();
            services.AddScoped<IRequestHandler<CompleteCourseCommand, bool>, CompleteCourseCommandHandler>();
            services.AddScoped<IRequestHandler<GenerateCertificateCommand, bool>, GenerateCertificateCommandHandler>();

            services.AddScoped<INotificationHandler<EnrollmentPaymentCompletedEvent>, EnrollmentEventHandler>();
            services.AddScoped<INotificationHandler<EnrollmentPaymentRejectedEvent>, EnrollmentEventHandler>();
            services.AddScoped<INotificationHandler<CourseCompletedEvent>, EnrollmentEventHandler>();

            // Payment
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICreditCardPaymentFacade, CreditCardPaymentFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<Finance.IConfigurationManager, Finance.ConfigurationManager>();
            services.AddScoped<INotificationHandler<PaymentStartedEvent>, PaymentEventHandler>();
            services.AddScoped<PaymentContext>();

            services.AddScoped<ICurrentUser, AppIdentityUser>();

            return services;
        }
    }
}
