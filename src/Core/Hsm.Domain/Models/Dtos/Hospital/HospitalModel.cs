using Hsm.Domain.Models.Dtos.Address;
using Hsm.Domain.Models.Dtos.City;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Hospital
{
    public class HospitalModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDto AddressDto { get; set; }
        public string ContactNumber { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }


        [PropertyMapping("CityModel")]
        public CityModel CityModel { get; set; }
    }
}
