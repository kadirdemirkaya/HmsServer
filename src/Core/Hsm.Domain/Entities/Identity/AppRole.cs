using Hsm.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace Hsm.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>, IBaseEntity
    {

        public DateTime CreatedDateUTC { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateUTC { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
