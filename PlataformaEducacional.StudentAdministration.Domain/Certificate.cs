using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.StudentAdministration.Domain
{
    public class Certificate : Entity
    {
        public Guid EnrollmentId { get; private set; }
        public string VerificationCode { get; private set; } = null!;
        public Enrollment Enrollment { get; private set; } = null!;

        protected Certificate() { }

        public Certificate(Guid enrollmentId)
        {
            EnrollmentId = enrollmentId;
            VerificationCode = Guid.NewGuid().ToString();
            Validate();
        }

        protected void Validate()
        {
            Validations.ValidateIfEmpty(EnrollmentId, "Enrollment ID is required.");
        }
    }
}
