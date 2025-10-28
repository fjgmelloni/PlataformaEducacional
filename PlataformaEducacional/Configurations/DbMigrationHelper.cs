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

namespace PlataformaEducacional.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelper.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelper
    {
        public static async Task EnsureSeedData(WebApplication application)
        {
            var service = application.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(service);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var contentContext = scope.ServiceProvider.GetRequiredService<ContentContext>();
            var studentContext = scope.ServiceProvider.GetRequiredService<StudentAdministrationContext>();
            var paymentContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

            if (env.EnvironmentName == "Development" || env.EnvironmentName == "Testing")
            {
                await identityContext.Database.MigrateAsync();
                await contentContext.Database.MigrateAsync();
                await studentContext.Database.MigrateAsync();
                await paymentContext.Database.MigrateAsync();

                await SeedUsersAndRoles(identityContext);
                await SeedContentTables(contentContext);
                await SeedStudentTables(identityContext, contentContext, studentContext, paymentContext);
            }
        }

        private static async Task SeedUsersAndRoles(ApplicationContext contextIdentity)
        {
            if (contextIdentity.Users.Any()) return;

            var roleAdmin = new IdentityRole
            {
                Name = "ADMIN",
                NormalizedName = "ADMIN"
            };

            var roleStudent = new IdentityRole
            {
                Name = "STUDENT",
                NormalizedName = "STUDENT"
            };

            await contextIdentity.Roles.AddAsync(roleAdmin);
            await contextIdentity.Roles.AddAsync(roleStudent);

            var adminId = Guid.NewGuid();
            var adminUser = new IdentityUser
            {
                Id = adminId.ToString(),
                Email = "admin@test.com",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@TEST.COM",
                UserName = "admin@test.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==",
                NormalizedUserName = "ADMIN@TEST.COM"
            };
            await contextIdentity.Users.AddAsync(adminUser);

            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = roleAdmin.Id,
                UserId = adminUser.Id
            });

            var studentId = Guid.NewGuid();
            var studentUser = new IdentityUser
            {
                Id = studentId.ToString(),
                Email = "student@test.com",
                EmailConfirmed = true,
                NormalizedEmail = "STUDENT@TEST.COM",
                UserName = "student@test.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==",
                NormalizedUserName = "STUDENT@TEST.COM"
            };
            await contextIdentity.Users.AddAsync(studentUser);

            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = roleStudent.Id,
                UserId = studentUser.Id
            });

            await contextIdentity.SaveChangesAsync();
        }

        private static async Task SeedContentTables(ContentContext contentContext)
        {
            if (!contentContext.Courses.Any())
            {
                var course = new Course(".NET", new Syllabus("Course Content", 30), 500, true);
                for (int i = 1; i <= 5; i++)
                {
                    var lesson = new Lesson($"Lesson {i}", $"Lesson {i} content", i, $"Material link for lesson {i}");
                    course.AddLesson(lesson);
                }
                await contentContext.Courses.AddAsync(course);

                course = new Course(".NET Core", new Syllabus("Content of .NET Core Course", 30), 500, true);
                var lessonCore = new Lesson("Lesson 1", "Lesson 1 content", 1, "Material link for lesson 1");
                course.AddLesson(lessonCore);
                await contentContext.Courses.AddAsync(course);

                course = new Course("Rich Domains", new Syllabus("Content of Rich Domains Course", 30), 500, true);
                var lessonRich = new Lesson("Lesson 1", "Lesson 1 content", 1, "Material link for lesson 1");
                course.AddLesson(lessonRich);
                await contentContext.Courses.AddAsync(course);

                await contentContext.SaveChangesAsync();
            }
        }

        private static async Task SeedStudentTables(
            ApplicationContext contextIdentity,
            ContentContext contentContext,
            StudentAdministrationContext studentContext,
            PaymentContext paymentContext)
        {
            if (!studentContext.Enrollments.Any())
            {
                var userStudent = await contextIdentity.Users.FirstOrDefaultAsync(x => x.Email == "student@test.com");
                var course = await contentContext.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Name == ".NET");
                var courseCore = await contentContext.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Name == ".NET Core");
                var courseRichDomains = await contentContext.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Name == "Rich Domains");

                var student = new Student(Guid.Parse(userStudent!.Id), "Student");

                var enrollment = new Enrollment(course!.Id, course.Name, course.Lessons.Count, 500);
                var enrollmentCore = new Enrollment(courseCore!.Id, courseCore.Name, courseCore.Lessons.Count, 500);
                var enrollmentRich = new Enrollment(courseRichDomains!.Id, courseRichDomains.Name, courseRichDomains.Lessons.Count, 500);

                student.EnrollInCourse(enrollment);
                student.EnrollInCourse(enrollmentCore);
                student.EnrollInCourse(enrollmentRich);

                var payment = new Payment(enrollment.Id, course.Price, new CardData("RINALDO", "111222333444", "05/27", "123"));
                var paymentCore = new Payment(enrollmentCore.Id, courseCore.Price, new CardData("RINALDO", "111222333444", "05/27", "123"));
                var paymentRich = new Payment(enrollmentRich.Id, courseRichDomains.Price, new CardData("RINALDO", "111222333444", "05/27", "123"));

                var transaction = new Transaction(payment.Id, payment.Amount);
                var transactionCore = new Transaction(paymentCore.Id, paymentCore.Amount);
                var transactionRich = new Transaction(paymentRich.Id, paymentRich.Amount);

                transaction.ChangeStatus(TransactionStatus.Paid);
                transactionCore.ChangeStatus(TransactionStatus.Paid);
                transactionRich.ChangeStatus(TransactionStatus.Paid);

                enrollment.Activate();
                enrollmentCore.Activate();
                enrollmentRich.Activate();

                foreach (var lesson in course.Lessons)
                {
                    var progress = new LessonProgress(lesson.Id);
                    enrollment.RecordLesson(progress);
                }
                enrollment.CompleteCourse();

                foreach (var lesson in courseRichDomains.Lessons)
                {
                    var progress = new LessonProgress(lesson.Id);
                    enrollmentRich.RecordLesson(progress);
                }

                await studentContext.Students.AddAsync(student);
                var certificate = new Certificate(enrollment.Id);
                await studentContext.Certificates.AddAsync(certificate);

                await paymentContext.Payments.AddRangeAsync(payment, paymentCore, paymentRich);
                await paymentContext.Transactions.AddRangeAsync(transaction, transactionCore, transactionRich);

                await paymentContext.SaveChangesAsync();
                await studentContext.SaveChangesAsync();
            }
        }
    }
}
