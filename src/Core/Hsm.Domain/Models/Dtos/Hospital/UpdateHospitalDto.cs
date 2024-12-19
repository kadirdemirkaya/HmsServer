using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.City;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Hospital
{
    public class UpdateHospitalDto
    {
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }


        public string Name { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;


        [PropertyMapping("CityModel")]
        public UpdateCityDto UpdateCityDto { get; set; }

    }
}
