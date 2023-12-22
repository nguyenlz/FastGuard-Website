using System;
using System.Collections.Generic;

namespace FastGuard.Models
{
    public partial class Coach
    {
        public Coach()
        {
            Routes = new HashSet<Route>();
        }

        public int CoachId { get; set; }
        public string? CoachNo { get; set; }
        public string? UserId { get; set; }
        public string? Supplier { get; set; }
        public int? Capacity { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Route> Routes { get; set; }
    }
}
