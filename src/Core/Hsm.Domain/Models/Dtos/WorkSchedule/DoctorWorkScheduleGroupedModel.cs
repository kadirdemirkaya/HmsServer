using Hsm.Domain.Models.Dtos.Doctor;

namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class DoctorWorkScheduleGroupedModel
    {
        public int WeekNumber { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public List<DoctorWorkScheduleByHour> SchedulesByHour { get; set; }
    }
    public class DoctorWorkScheduleByHour
    {
        public int Hour { get; set; }
        public List<DoctorWorkScheduleAppointmentsModel> Schedules { get; set; }
    }
}
