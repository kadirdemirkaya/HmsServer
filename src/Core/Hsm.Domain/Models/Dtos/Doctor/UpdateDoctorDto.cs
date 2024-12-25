using Hsm.Domain.Models.Dtos.Hospital;

namespace Hsm.Domain.Models.Dtos.Doctor
{
    public class UpdateDoctorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string Schedule { get; set; }
        //public byte[] RowVersion { get; set; } = null!;
        public Guid RowVersion { get; set; }
        public bool IsActive { get; set; }
    }
}
