using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Api.Data;
using PlataformaEducacional.ContentManagement.Data.Context;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.ContentManagement.Domain.ValueObjects;
using PlataformaEducacional.FinancialManagement.Core;
using PlataformaEducacional.FinancialManagement.Data;
using PlataformaEducacional.StudentAdministration.Data;
using PlataformaEducacional.StudentAdministration.Domain;
using System.Threading.Tasks;
using System;

namespace PlataformaEducacional.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static async Task UseDbMigrationHelperAsync(this WebApplication app)
        {
            await DbMigrationHelper.EnsureSeedData(app);
        }
    }

    public static class DbMigrationHelper
    {
        public static async Task EnsureSeedData(WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            await EnsureSeedData(scope.ServiceProvider);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            if (env.EnvironmentName is not ("Development" or "Testing"))
                return;

            var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var contentContext = scope.ServiceProvider.GetRequiredService<ContentContext>();
            var studentContext = scope.ServiceProvider.GetRequiredService<StudentAdministrationContext>();
            var paymentContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

            await identityContext.Database.MigrateAsync();
            await contentContext.Database.MigrateAsync();
            await studentContext.Database.MigrateAsync();
            await paymentContext.Database.MigrateAsync();

            await SeedUsersAndRoles(scope.ServiceProvider);
            await SeedContentTables(contentContext);
            await SeedStudentTables(identityContext, contentContext, studentContext, paymentContext);
        }

        // =========================
        // USERS & ROLES
        // =========================
        private static async Task SeedUsersAndRoles(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            async Task EnsureRole(string roleName)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await EnsureRole("ADMIN");
            await EnsureRole("STUDENT");

            async Task EnsureUser(string email, string password, string role)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null) return;

                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    throw new Exception($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                await userManager.AddToRoleAsync(user, role);
            }

            await EnsureUser("admin@test.com", "Teste@123", "ADMIN");
            await EnsureUser("student@test.com", "Teste@123", "STUDENT");
        }

        // =========================
        // CONTENT
        // =========================
        private static async Task SeedContentTables(ContentContext contentContext)
        {
            if (contentContext.Courses.Any())
                return;

            var dotNet = new Course(".NET", new Syllabus("Course Content", 30), 500, true);
            for (int i = 1; i <= 5; i++)
                dotNet.AddLesson(new Lesson($"Lesson {i}", $"Lesson {i} content", i, $"Material {i}"));

            var dotNetCore = new Course(".NET Core", new Syllabus("Content of .NET Core Course", 30), 500, true);
            dotNetCore.AddLesson(new Lesson("Lesson 1", "Lesson 1 content", 1, "Material 1"));

            var richDomains = new Course("Rich Domains", new Syllabus("Content of Rich Domains Course", 30), 500, true);
            richDomains.AddLesson(new Lesson("Lesson 1", "Lesson 1 content", 1, "Material 1"));

            await contentContext.Courses.AddRangeAsync(dotNet, dotNetCore, richDomains);
            await contentContext.SaveChangesAsync();
        }

        // =========================
        // STUDENT / PAYMENT
        // =========================
        private static async Task SeedStudentTables(
            ApplicationContext identityContext,
            ContentContext contentContext,
            StudentAdministrationContext studentContext,
            PaymentContext paymentContext)
        {
            if (studentContext.Students.Any())
                return;

            var user = await identityContext.Users.FirstAsync(u => u.Email == "student@test.com");
            var courses = await contentContext.Courses.Include(c => c.Lessons).ToListAsync();

            var student = new Student(Guid.Parse(user.Id), "Student");

            foreach (var course in courses)
            {
                var enrollment = new Enrollment(course.Id, course.Name, course.Lessons.Count, course.Price);
                student.EnrollInCourse(enrollment);

                var payment = new Payment(
                    enrollment.Id,
                    course.Price,
                    new CardData("TEST USER", "4111111111111111", "12/30", "123")
                );

                var transaction = new Transaction(payment.Id, payment.Amount);
                transaction.ChangeStatus(TransactionStatus.Paid);

                enrollment.Activate();

                if (course.Name == ".NET")
                {
                    foreach (var lesson in course.Lessons)
                        enrollment.RecordLesson(new LessonProgress(lesson.Id));

                    enrollment.CompleteCourse();
                    await studentContext.Certificates.AddAsync(new Certificate(enrollment.Id));
                }

                await paymentContext.Payments.AddAsync(payment);
                await paymentContext.Transactions.AddAsync(transaction);
            }

            await studentContext.Students.AddAsync(student);

            await paymentContext.SaveChangesAsync();
            await studentContext.SaveChangesAsync();
        }
    }
}
