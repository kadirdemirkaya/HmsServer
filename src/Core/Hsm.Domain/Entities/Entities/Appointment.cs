using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;

namespace Hsm.Domain.Entities.Entities
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentTime { get; set; }

        public Guid UserId { get; set; }
        public Guid WorkScheduleId { get; set; }

        public AppUser User { get; set; }
        public WorkSchedule WorkSchedule { get; set; }

        public Appointment()
        {

        }

        public Appointment(DateTime appointmentTime, Guid userId, Guid workScheduleId)
        {
            CreateId();
            SetAppointmentTime(appointmentTime)
           .SetUserId(userId)
           .SetWorkScheduleId(workScheduleId);
        }

        public Appointment(Guid id, DateTime appointmentTime, Guid userId, Guid workScheduleId)
        {
            SetId(id);
            SetAppointmentTime(appointmentTime)
           .SetUserId(userId)
           .SetWorkScheduleId(workScheduleId);
        }

        public static Appointment Create(DateTime appointmentTime, Guid userId, Guid workScheduleId)
            => new(appointmentTime, userId, workScheduleId);
        public static Appointment Create(Guid id, DateTime appointmentTime, Guid userId, Guid workScheduleId)
            => new(id, appointmentTime, userId, workScheduleId);

        public Appointment SetAppointmentTime(DateTime appointmentTime) { AppointmentTime = appointmentTime; return this; }
        public Appointment SetUserId(Guid userId) { UserId = userId; return this; }
        public Appointment SetWorkScheduleId(Guid workScheduleId) { WorkScheduleId = workScheduleId; return this; }
    }
}
