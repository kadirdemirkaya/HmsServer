using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;

namespace Hsm.Domain.Entities.Entities
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ContactInfo { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<ElectronicHealthRecord> MedicalRecords { get; set; } = new List<ElectronicHealthRecord>();
    }
}
