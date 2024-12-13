using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hsm.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        public string TcNumber { get; set; }
        public DateTime CreatedDateUTC { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateUTC { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Doctor> Doctors { get; set; }

    }
}
