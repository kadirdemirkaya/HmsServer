using Hsm.Domain.Models.Dtos.Appointment;

namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class DoctorWorkScheduleAppointmentsModel
    {
        public Guid WorkScheduleId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RowVersion { get; set; }
        public bool IsActive { get; set; }

        public List<AppointmentModel> AppointmentModels { get; set; }
    }
}
