using Hsm.Domain.Entities.Base;
using ModelMapper;

namespace Hsm.Domain.Entities.Entities
{
    public class Hospital : BaseEntity
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string ContactNumber { get; set; }

        public Guid CityId { get; set; }

        [PropertyMapping("CityModel")]
        public City City { get; set; }

        public ICollection<Doctor> Doctors { get; set; }

    }
}
