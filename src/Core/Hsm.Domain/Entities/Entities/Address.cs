using Microsoft.EntityFrameworkCore;

namespace Hsm.Domain.Entities.Entities
{
    [Owned]
    public class Address
    {
        public string? Street { get; private set; }
        public string District { get; private set; } // il.e
        public string City { get; private set; } // il
        public string? State { get; private set; }
        public string? PostalCode { get; private set; }
        public string Country { get; private set; }


        public Address()
        {

        }

        public Address(string street, string city, string state, string postalCode, string country,string disctrict)
        {
            SetStreet(street)
           .SetCity(city)
           .SetState(state)
           .SetPostalCode(postalCode)
           .SetCountry(country)
           .SetDistrict(disctrict);
        }

        public static Address Create(string street, string city, string state, string postalCode, string country,string district)
            => new(street, city, state, postalCode, country,district);


        public Address SetStreet(string street) { Street = street; return this; }
        public Address SetCity(string city) { City = city; return this; }
        public Address SetState(string state) { State = state; return this; }
        public Address SetPostalCode(string postalCode) { PostalCode = postalCode; return this; }
        public Address SetCountry(string country) { Country = country; return this; }
        public Address SetDistrict(string district) { District = district; return this; }
    }
}
