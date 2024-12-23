namespace Hsm.Domain.Models.Dtos.Clinic
{
    public class ClinicalModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid HospitalId { get; set; }
    }
}
