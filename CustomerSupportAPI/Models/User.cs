﻿namespace CustomerSupportAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
    }
}
