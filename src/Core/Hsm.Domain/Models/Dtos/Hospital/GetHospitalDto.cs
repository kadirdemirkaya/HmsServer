namespace Hsm.Domain.Models.Dtos.Hospital
{
    public class GetHospitalDto
    {
        public string Province { get; set; }
        public string? District { get; set; }
        public Guid? ClinicalId { get; set; }
    }
}
