using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.City;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Hospital
{
    public class CreateHospitalDto
    {

        public string Name { get; set; }
        public Address Address { get; set; }
        public string ContactNumber { get; set; }


        [PropertyMapping("CityModel")]
        public CreateCityDto CreateCityDto { get; set; }
    }
}
