using Hsm.Domain.Models.Dtos.Doctor;

namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class SearchWorkScheduleModel
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string HospitalName { get; set; }
        public string PolicinicalName { get; set; }
        public Hsm.Domain.Entities.Entities.Address HospitalAddress { get; set; }

        public NearestWorkScheduleModel NearestWorkScheduleModel { get; set; }
    }
}
