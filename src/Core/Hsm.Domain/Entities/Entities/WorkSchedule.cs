using Hsm.Domain.Entities.Base;
using Hsm.Domain.Models.Dtos.Appointment;
using ModelMapper;

namespace Hsm.Domain.Entities.Entities
{
    public class WorkSchedule : BaseEntity
    {
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public Guid DoctorId { get; private set; }

        [PropertyMapping("DoctorModel")]
        public Doctor Doctor { get; private set; }

        [PropertyMapping("AppointmentModel")]
        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();

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

        public string GetStartDateForMail() => StartDate.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:ss");
        public void AddAppointmentToWorkSchedule(Appointment appointment)
        {
            Appointments.Add(appointment);
        }

        public void AddAppointmentToWorkSchedule(List<Appointment> appointments)
        {
            foreach (var appointment in appointments)
            {
                Appointments.Add(appointment);
            }
        }

        public void UpdateAppointment(List<AppointmentModel> appointmentModels)
        {
            foreach (var appointmentModel in appointmentModels)
            {
                var existingAppointment = Appointments.FirstOrDefault(a => a.Id == appointmentModel.Id);

                if (existingAppointment != null)
                {
                    existingAppointment.SetRowVersion(appointmentModel.RowVersion);
                    existingAppointment.SetIsActive(appointmentModel.IsActive);
                    existingAppointment.SetAppointmentTime(appointmentModel.AppointmentTime);
                    existingAppointment.SetUserId(appointmentModel.UserId);
                }
            }
        }

        public WorkSchedule SetName(string name) { Name = name; return this; }
        public WorkSchedule SetStartDate(DateTime startDate) { StartDate = startDate; return this; }
        public WorkSchedule SetEndDate(DateTime endDate) { EndDate = endDate; return this; }
        public WorkSchedule SetDoctorId(Guid doctorId) { DoctorId = doctorId; return this; }
        public WorkSchedule SetDoctor(Doctor doctor) { Doctor = doctor; return this; }
    }
}
