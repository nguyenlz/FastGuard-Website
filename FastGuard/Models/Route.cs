using System;
using System.Collections.Generic;

namespace FastGuard.Models
{
    public partial class Route
    {
        public Route()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int RouteId { get; set; }
        public int? CoachId { get; set; }
        public int? LocationId1 { get; set; }
        public int? LocationId2 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float? Price { get; set; }

        public virtual Coach? Coach { get; set; }
        public virtual Location? LocationId1Navigation { get; set; }
        public virtual Location? LocationId2Navigation { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
