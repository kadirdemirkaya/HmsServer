using Hsm.Domain.Entities.Base;
using Hsm.Domain.Models.Dtos.City;
using ModelMapper;
using System.Xml.Linq;

namespace Hsm.Domain.Entities.Entities
{
    public class WorkSchedule : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid DoctorId { get; set; }

        [PropertyMapping("DoctorModel")]
        public Doctor Doctor { get; set; }

        [PropertyMapping("AppointmentModel")]
        public ICollection<Appointment> Appointments { get; set; }

        public WorkSchedule()
        {

        }

        public WorkSchedule(string name, DateTime startDate, DateTime endDate, Guid doctorId)
        {
            CreateId();
            SetName(name)
           .SetStartDate(startDate)
           .SetEndDate(endDate)
           .SetDoctorId(doctorId);
        }
        public WorkSchedule(Guid id, string name, DateTime startDate, DateTime endDate, Guid doctorId)
        {
            SetId(id);
            SetName(name)
           .SetStartDate(startDate)
           .SetEndDate(endDate)
           .SetDoctorId(doctorId);
        }

        public static WorkSchedule Create(string name, DateTime startDate, DateTime endDate, Guid doctorId)
            => new(name, startDate, endDate, doctorId);

        public static WorkSchedule Create(Guid id, string name, DateTime startDate, DateTime endDate, Guid doctorId)
            => new(id, name, startDate, endDate, doctorId);

        public void AddAppointmentToWorkSchedule(Appointment appointment)
        {
            Appointments.Add(appointment);
        }

        public WorkSchedule SetName(string name) { Name = name; return this; }
        public WorkSchedule SetStartDate(DateTime startDate) { StartDate = startDate; return this; }
        public WorkSchedule SetEndDate(DateTime endDate) { EndDate = endDate; return this; }
        public WorkSchedule SetDoctorId(Guid doctorId) { DoctorId = doctorId; return this; }
    }
}
