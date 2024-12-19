using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;
using ModelMapper;

namespace Hsm.Domain.Entities.Entities
{
    public class Doctor : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Specialty { get; private set; }
        public string Schedule { get; private set; }

        public Guid HospitalId { get; private set; }

        [PropertyMapping("HospitalModel")]
        public Hospital Hospital { get; private set; }


        public Guid AppUserId { get; private set; }
        public AppUser AppUser { get; private set; }


        public ICollection<WorkSchedule> WorkSchedules { get; private set; }

        public Doctor()
        {

        }

        public Doctor(string firstName, string lastName, string specialty, string schedule, Guid appUserId)
        {
            CreateId();
            SetFirstName(firstName)
           .SetLastName(lastName)
           .SetSpecialty(specialty)
           .SetSchedule(schedule)
           .SetAppUserId(appUserId);
        }

        public Doctor(Guid id, string firstName, string lastName, string specialty, string schedule, Guid appUserId) : base(id)
        {
            SetId(id);
            SetFirstName(firstName)
           .SetLastName(lastName)
           .SetSpecialty(specialty)
           .SetSchedule(schedule)
           .SetAppUserId(appUserId);
        }

        public static Doctor Create(string firstName, string lastName, string specialty, string schedule, Guid appUserId)
           => new(firstName, lastName, specialty, schedule, appUserId);
        public static Doctor Create(Guid id, string firstName, string lastName, string specialty, string schedule, Guid appUserId)
            => new(id, firstName, lastName, specialty, schedule, appUserId);

        public Doctor SetAppUserId(Guid appUserId) { AppUserId = appUserId; return this; }
        public Doctor SetFirstName(string firstName) { FirstName = firstName; return this; }
        public Doctor SetLastName(string lastName) { LastName = lastName; return this; }
        public Doctor SetSpecialty(string specialty) { Specialty = specialty; return this; }
        public Doctor SetSchedule(string schedule) { Schedule = schedule; return this; }
        public Doctor SetHospital(Hospital hospital)
        {


            return this;
        }
    }
}
