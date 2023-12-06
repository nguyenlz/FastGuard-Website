using System;
using System.Collections.Generic;

namespace FastGuard.Data
{
    public partial class User
    {
        public User()
        {
            Coaches = new HashSet<Coach>();
            Invoices = new HashSet<Invoice>();
        }

        public int UserId { get; set; }
        public string? Name { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
        public int? AccountStatusId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Coach> Coaches { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
