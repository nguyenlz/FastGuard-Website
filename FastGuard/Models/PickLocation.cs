using System;
using System.Collections.Generic;

namespace FastGuard.Models
{
    public partial class PickLocation
    {
        public PickLocation()
        {
            TicketPickLocationId1Navigations = new HashSet<Ticket>();
            TicketPickLocationId2Navigations = new HashSet<Ticket>();
        }

        public int PickLocationId { get; set; }
        public string? PickLocationName { get; set; }
        public int? LocationId { get; set; }

        public virtual Location? Location { get; set; }
        public virtual ICollection<Ticket> TicketPickLocationId1Navigations { get; set; }
        public virtual ICollection<Ticket> TicketPickLocationId2Navigations { get; set; }
    }
}
