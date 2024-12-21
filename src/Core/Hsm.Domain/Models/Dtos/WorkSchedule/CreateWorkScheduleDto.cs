namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class CreateWorkScheduleDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid DoctorId { get; set; }
    }
}
