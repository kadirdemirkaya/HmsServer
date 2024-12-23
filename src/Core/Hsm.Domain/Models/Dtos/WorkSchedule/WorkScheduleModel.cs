﻿using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.Doctor;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class WorkScheduleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }

        [PropertyMapping("DoctorModel")]
        public DoctorModel DoctorModel { get; set; }

        [PropertyMapping("AppointmentModel")]
        public ICollection<AppointmentModel> AppointmentModels { get; set; }
    }
}
