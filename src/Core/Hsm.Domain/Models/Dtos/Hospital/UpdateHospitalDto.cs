using Hsm.Domain.Models.Dtos.Address;
using Hsm.Domain.Models.Dtos.City;
using ModelMapper;
using System.ComponentModel.DataAnnotations;

namespace Hsm.Domain.Models.Dtos.Hospital
{
    public class UpdateHospitalDto
    {
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsActive { get; set; }


        public string Name { get; set; } = null!;

        [PropertyMapping("Address")]
        public AddressDto AddressDto { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;


        [PropertyMapping("CityModel")]
        public UpdateCityDto UpdateCityDto { get; set; }

    }
}
