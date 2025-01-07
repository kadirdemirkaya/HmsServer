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

        public string FormatUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return userName;

            int upperCaseCount = 0;
            for (int i = 0; i < userName.Length; i++)
            {
                if (char.IsUpper(userName[i]))
                {
                    upperCaseCount++;
                    if (upperCaseCount == 2)
                    {
                        return userName.Insert(i, " ");
                    }
                }
            }
            return userName;
        }

        public (string firstName, string lastName) FormatUserNameAndLastName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return (userName, string.Empty);

            int upperCaseCount = 0;
            for (int i = 0; i < userName.Length; i++)
            {
                if (char.IsUpper(userName[i]))
                {
                    upperCaseCount++;
                    if (upperCaseCount == 2)
                    {
                        string firstName = userName.Substring(0, i);
                        string lastName = userName.Substring(i);
                        return (firstName, lastName);
                    }
                }
            }
            return (userName, string.Empty);
        }
    }
}
