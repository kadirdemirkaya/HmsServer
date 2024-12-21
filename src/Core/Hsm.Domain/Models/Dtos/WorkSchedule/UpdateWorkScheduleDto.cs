using Hsm.Domain.Models.Dtos.Appointment;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class UpdateWorkScheduleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }


        [PropertyMapping("AppointmentModel")]
        public List<AppointmentModel> AppointmentModels { get; set; }
    }
}
