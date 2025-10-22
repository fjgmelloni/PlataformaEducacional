using Microsoft.Extensions.DependencyInjection;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate;

namespace PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands
{
    public static class StudentsFeatureRegistration
    {
        public static IServiceCollection AddStudentsFeature(this IServiceCollection services)
        {
            // Register Command Handlers
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(AddStudentCommandHandler).Assembly,
                    typeof(CancelEnrollmentCommandHandler).Assembly,
                    typeof(CompleteCourseCommandHandler).Assembly,
                    typeof(CompleteEnrollmentCommandHandler).Assembly,
                    typeof(EnrollInCourseCommandHandler).Assembly,
                    typeof(GenerateCertificateCommandHandler).Assembly
                );
            });

            return services;
        }
    }
}
