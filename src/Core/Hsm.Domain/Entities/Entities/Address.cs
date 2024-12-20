using Microsoft.EntityFrameworkCore;

namespace Hsm.Domain.Entities.Entities
{
    [Owned]
    public class Address
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }


        public Address()
        {

        }

        public Address(string street, string city, string state, string postalCode, string country)
        {
            SetStreet(street)
           .SetCity(city)
           .SetState(state)
           .SetPostalCode(postalCode)
           .SetCountry(country);
        }

        public static Address Create(string street, string city, string state, string postalCode, string country)
            => new(street, city, state, postalCode, country);


        public Address SetStreet(string street) { Street = street; return this; }
        public Address SetCity(string city) { City = city; return this; }
        public Address SetState(string state) { State = state; return this; }
        public Address SetPostalCode(string postalCode) { PostalCode = postalCode; return this; }
        public Address SetCountry(string country) { Country = country; return this; }
    }
}
