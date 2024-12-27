using Hsm.Domain.Models.Dtos.User;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Appointment
{
    public class UserAppointmentsModel
    {
        public Guid Id { get; set; }
        public DateTime AppointmentTime { get; set; }

        [PropertyMapping("UserModel")]
        public UserModel UserModel { get; set; }

        [PropertyMapping("WorkScheduleModel")]
        public WorkScheduleModel WorkScheduleModel { get; set; }
    }
}
