using Hsm.Domain.Models.Dtos.Doctor;

namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class SearchWorkScheduleDto
    {
        public string Province { get; set; }
        public string? District { get; set; }
        public Guid ClinicId { get; set; }
        public Guid? HospitalId { get; set; }
        public Guid? DoctorId { get; set; }
    }
}
