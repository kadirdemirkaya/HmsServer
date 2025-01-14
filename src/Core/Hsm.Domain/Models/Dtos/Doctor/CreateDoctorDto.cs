﻿using Hsm.Domain.Models.Dtos.Hospital;
using ModelMapper;

namespace Hsm.Domain.Models.Dtos.Doctor
{
    public class CreateDoctorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string Schedule { get; set; }

        public Guid HospitalId { get; set; }
    }
}
