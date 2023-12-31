﻿using Microsoft.AspNetCore.Identity;

namespace FastGuard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double? salary { get; set; }
    }
}
