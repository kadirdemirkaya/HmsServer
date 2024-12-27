namespace Hsm.Domain.Models.Dtos.Appointment
{
    public class SearchAppointmentDto
    {
        public string Province { get; set; }
        public string? District { get; set; } = null;
        public Guid? HospitalId { get; set; } = null;
        public Guid? ClinicId { get; set; } = null;
        public Guid? DoctorId { get; set; } = null;
    }
}
