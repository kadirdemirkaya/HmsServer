using Hsm.Domain.Models.Dtos.Hospital;

namespace Hsm.Domain.Models.Dtos.Clinic
{
    public class ClinicalHospitalsModel
    {
        public ClinicalModel ClinicalModel { get; set; }
        public HospitalModel HospitalModel { get; set; }
    }
}
