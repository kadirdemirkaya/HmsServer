﻿namespace Hsm.Domain.Models.Dtos.User
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Token { get; set; }
    }
}
