using Hsm.Domain.Models.Dtos.Hospital;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Doctor
{
    public class DoctorModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string Schedule { get; set; }
        //public byte[] RowVersion { get; set; } = null!;
        public Guid RowVersion { get; set; }
        public bool IsActive { get; set; }


        [PropertyMapping("HospitalModel")]
        public HospitalModel Hospital { get; set; }


        public Guid AppUserId { get; set; }

    }
}
