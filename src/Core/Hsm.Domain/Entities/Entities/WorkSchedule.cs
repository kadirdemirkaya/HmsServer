using Hsm.Domain.Entities.Base;

namespace Hsm.Domain.Entities.Entities
{
    public class WorkSchedule : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
