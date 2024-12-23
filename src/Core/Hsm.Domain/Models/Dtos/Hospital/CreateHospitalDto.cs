using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Address;
using Hsm.Domain.Models.Dtos.City;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Hospital
{
    public class CreateHospitalDto
    {

        public string Name { get; set; }
        [PropertyMapping("Address")]
        public AddressDto AddressDto { get; set; }
        public string ContactNumber { get; set; }


        [PropertyMapping("CityModel")]
        public CreateCityDto CreateCityDto { get; set; }
    }
}
