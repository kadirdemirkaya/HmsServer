using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;
using ModelMapper;

namespace Hsm.Domain.Entities.Entities
{
    public class Doctor : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string Schedule { get; set; }

        public Guid HospitalId { get; set; }

        [PropertyMapping("HospitalModel")]
        public Hospital Hospital { get; set; }


        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public ICollection<WorkSchedule> WorkSchedules { get; set; }
    }
}
