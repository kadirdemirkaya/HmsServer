using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;

namespace Hsm.Domain.Entities.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; private set; }
        public int NotificationType { get; private set; }

        public Guid UserId { get; private set; }
        public AppUser User { get; private set; }

        public Notification()
        {

        }

        public Notification(string message, int notificationType, Guid userId)
        {
            CreateId();
            SetMessage(message)
           .SetNotificationType(notificationType)
           .SetUserId(userId);
        }

        public Notification(Guid id, string message, int notificationType, Guid userId) : base(id)
        {
            SetId(id);
            SetMessage(message)
           .SetNotificationType(notificationType)
           .SetUserId(userId);
        }

        public static Notification Create(string message, int notificationType, Guid userId)
            => new(message,notificationType,userId);
        public static Notification Create(Guid id, string message, int notificationType, Guid userId)
            => new(id, message, notificationType, userId);

        public Notification SetMessage(string message) { Message = message; return this; }
        public Notification SetNotificationType(int notificationType) { NotificationType = notificationType; return this; }
        public Notification SetUserId(Guid userId) { UserId = userId; return this; }

    }
}
