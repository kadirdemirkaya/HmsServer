 using Hsm.Domain.Entities.Base;
using ModelMapper;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Loader;

namespace Hsm.Domain.Entities.Entities
{
    public class Hospital : BaseEntity
    {
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public string ContactNumber { get; private set; }

        public Guid CityId { get; private set; }

        [PropertyMapping("CityModel")]
        public City City { get; private set; }

        public ICollection<Doctor> Doctors { get; private set; }


        public Hospital()
        {

        }

        public Hospital(string name, Address address, string contactNumber, Guid cityId)
        {
            CreateId();
            SetName(name)
           .SetAddress(address)
           .SetContactNumber(contactNumber)
           .SetCityId(cityId);
        }

        public Hospital(Guid id, string name, Address address, string contactNumber, Guid cityId) : base(id)
        {
            SetId(id);
            SetName(name)
           .SetAddress(address)
           .SetContactNumber(contactNumber)
           .SetCityId(cityId);
        }

        public static Hospital Create(string name, Address address, string contactNumber, Guid cityId)
            => new(name, address, contactNumber, cityId);

        public static Hospital Create(Guid id, string name, Address address, string contactNumber, Guid cityId)
          => new(id, name, address, contactNumber, cityId);

        public void AddDoctorToHospital(Doctor doctor)
        {
            Doctors.Add(doctor);
        }

        public Hospital SetName(string name) { Name = name; return this; }
        public Hospital SetAddress(Address address) { Address = address; return this; }
        public Hospital SetContactNumber(string contactNumber) { ContactNumber = contactNumber; return this; }
        public Hospital SetCityId(Guid cityId) { CityId = cityId; return this; }

    }
}
