using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;

namespace Hsm.Domain.Entities.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public int NotificationType { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
