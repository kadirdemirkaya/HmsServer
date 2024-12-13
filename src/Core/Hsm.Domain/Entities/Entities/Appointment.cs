﻿using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;

namespace Hsm.Domain.Entities.Entities
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentTime { get; set; }

        public Guid UserId { get; set; }
        public Guid WorkScheduleId { get; set; }

        public AppUser User { get; set; }
        public WorkSchedule WorkSchedule { get; set; }
    }
}
