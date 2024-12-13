using Hsm.Domain.Entities.Base;

namespace Hsm.Domain.Entities.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Hospital> Hospitals { get; set; }
    }
}
