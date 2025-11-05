using Bogus;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Domain.Tests
{
    public class StudentTests
    {
        private Faker Faker { get; set; } = new Faker("pt_BR");

        private readonly Guid _validStudentId = Guid.NewGuid();
        private readonly string _validStudentName = "Felício";

        private Student CreateValidStudent()
        {
            return new Student(_validStudentId, _validStudentName);
        }

        [Theory(DisplayName = "Create Student with invalid name")]
        [Trait("Category", "Student - Constructor")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrowException_WhenNameInvalid(string invalidName)
        {
            // Act
            var ex = Assert.Throws<DomainException>(() => new Student(Guid.NewGuid(), invalidName));

            // Assert
            Assert.Equal("The student name is required.", ex.Message);
        }

        [Fact(DisplayName = "Create Student with invalid Id")]
        [Trait("Category", "Student - Constructor")]
        public void Constructor_ShouldThrowException_WhenIdInvalid()
        {
            // Arrange
            var name = Faker.Person.FirstName;

            // Act
            var ex = Assert.Throws<DomainException>(() => new Student(Guid.Empty, name));

            // Assert
            Assert.Equal("The student ID is required.", ex.Message);
        }

        [Fact(DisplayName = nameof(Constructor_Valid))]
        [Trait("Category", "Student - Constructor")]
        public void Constructor_Valid()
        {
            // Arrange
            var name = Faker.Person.FirstName;
            var studentId = Guid.NewGuid();

            // Act 
            var student = new Student(studentId, name);

            // Assert
            Assert.Equal(name, student.Name);
            Assert.Equal(studentId, student.Id);
        }

        [Fact(DisplayName = "EnrollInCourse should add Enrollment to list")]
        [Trait("Category", "Student - Enrollment")]
        public void Student_EnrollInCourse_ShouldAddEnrollment()
        {
            // Arrange
            var student = CreateValidStudent();
            var courseId = Guid.NewGuid();
            var newEnrollment = new Enrollment(courseId, "C# Course", 250, 500);

            // Act
            student.EnrollInCourse(newEnrollment);

            // Assert
            Assert.Single(student.Enrollments);
            Assert.Equal(courseId, student.Enrollments.First().CourseId);
            Assert.Equal(_validStudentId, student.Enrollments.First().StudentId);
        }

        [Fact(DisplayName = "EnrollInCourse should throw exception when already enrolled")]
        [Trait("Category", "Student - Enrollment")]
        public void Student_EnrollInCourse_ShouldThrow_WhenAlreadyEnrolled()
        {
            // Arrange
            var student = CreateValidStudent();
            var courseId = Guid.NewGuid();
            var existing = new Enrollment(courseId, "C# Course", 250, 500);
            student.EnrollInCourse(existing);

            var duplicate = new Enrollment(courseId, "C# Course", 250, 500);

            // Act
            var ex = Assert.Throws<DomainException>(() => student.EnrollInCourse(duplicate));

            // Assert
            Assert.Equal("Student already enrolled in this course.", ex.Message);
            Assert.Single(student.Enrollments);
        }

        [Fact(DisplayName = "HasEnrollment should return true when enrollment exists")]
        [Trait("Category", "Student - Enrollment")]
        public void Student_HasEnrollment_ShouldReturnTrue_WhenEnrollmentExists()
        {
            // Arrange
            var student = CreateValidStudent();
            var courseId = Guid.NewGuid();
            var enrollment = new Enrollment(courseId, "C# Course", 250, 500);
            student.EnrollInCourse(enrollment);

            var check = new Enrollment(courseId, "C# Course", 250, 500);

            // Act
            var result = student.HasEnrollment(check);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "HasEnrollment should return false when enrollment does not exist")]
        [Trait("Category", "Student - Enrollment")]
        public void Student_HasEnrollment_ShouldReturnFalse_WhenEnrollmentDoesNotExist()
        {
            // Arrange
            var student = CreateValidStudent();
            var check = new Enrollment(Guid.NewGuid(), "Angular Course", 250, 500);

            // Act
            var result = student.HasEnrollment(check);

            // Assert
            Assert.False(result);
        }
    }
}
