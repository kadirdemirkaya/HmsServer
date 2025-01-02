namespace Hsm.Domain.Models.Dtos.Appointment
{
    public class TakeAppointmentDto
    {
        public DateTime AppointmentTime { get; set; }
        public Guid WorkScheduleId { get; set; }
    }
}
