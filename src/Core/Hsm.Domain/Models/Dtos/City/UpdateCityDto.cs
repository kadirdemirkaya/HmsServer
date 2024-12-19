namespace Hsm.Domain.Models.Dtos.City
{
    public class UpdateCityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
