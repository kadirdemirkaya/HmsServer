namespace Hsm.Domain.Models.Dtos.Address
{
    public class AddressDto
    {
        public string Street { get; set; }
        public string District { get; set; } // il.e
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
