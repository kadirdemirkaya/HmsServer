using Hsm.Domain.Models.Dtos.Doctor;

namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class DoctorWorkScheduleModel
    {
        public DoctorModel DoctorModel { get; set; }
        public List<DoctorWorkScheduleAppointmentsModel> DoctorWorkScheduleAppointmentsModels { get; set; }
    }
}
