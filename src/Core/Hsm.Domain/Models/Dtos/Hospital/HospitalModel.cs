using Hsm.Domain.Models.Dtos.City;
using Hsm.Domain.Models.Dtos.Clinic;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Hospital
{
    public class HospitalModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [PropertyMapping("Address")]
        public Hsm.Domain.Entities.Entities.Address Address { get; set; }
        public string ContactNumber { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }


        [PropertyMapping("CityModel")]
        public CityModel CityModel { get; set; }

        [PropertyMapping("ClinicalModel")]
        public List<ClinicalModel> ClinicalModels { get; set; }
    }
}
