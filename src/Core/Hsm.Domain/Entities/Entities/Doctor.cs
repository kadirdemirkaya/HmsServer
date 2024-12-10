using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;

namespace Hsm.Domain.Entities.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Specialization { get; set; }
        public string Schedule { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
