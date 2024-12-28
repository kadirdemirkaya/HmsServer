namespace Hsm.Domain.Models.Dtos.Doctor
{
    public class GetDoctorDto
    {
        public Guid HospitalId { get; set; }
        public Guid ClinicalId { get; set; }
    }
}
