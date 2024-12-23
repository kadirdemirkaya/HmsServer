namespace Hsm.Domain.Models.Dtos.Appointment
{
    public class SearchAppointmentDto
    {
        public string Province { get; set; }
        public string District { get; set; }
        public Guid? HospitalId { get; set; }
        public Guid? ClinicId { get; set; }
        public Guid? WorkScheduleId { get; set; }
    }
}
