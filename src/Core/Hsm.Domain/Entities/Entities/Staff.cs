using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;

namespace Hsm.Domain.Entities.Entities
{
    public class Staff : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
