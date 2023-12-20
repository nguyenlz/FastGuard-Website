using System;
using System.Collections.Generic;

namespace FastGuard.Models
{
    public partial class Ticket
    {
        public int InvoiceId { get; set; }
        public string UserId { get; set; }
        public string SeatNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int PickLocationId1 { get; set; }
        public int PickLocationId2 { get; set; }
        public int RouteId { get; set; }

        public virtual PickLocation PickLocationId1Navigation { get; set; }
        public virtual PickLocation PickLocationId2Navigation { get; set; }
        public virtual Route Route { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
