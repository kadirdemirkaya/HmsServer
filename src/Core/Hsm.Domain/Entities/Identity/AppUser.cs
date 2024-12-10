using Hsm.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace Hsm.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        public DateTime CreatedDateUTC { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateUTC { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
