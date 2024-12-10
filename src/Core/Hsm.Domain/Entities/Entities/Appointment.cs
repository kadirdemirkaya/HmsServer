using Hsm.Domain.Entities.Base;

namespace Hsm.Domain.Entities.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public bool Status { get; set; }
    }
}
