﻿namespace Hsm.Domain.Models.Options
{
    public class EmailOptions
    {
        public string From { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
