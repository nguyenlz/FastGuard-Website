using System;
using System.Collections.Generic;

namespace FastGuard.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int TicketId { get; set; }
        public int? RouteId { get; set; }
        public DateTime? BookingDate { get; set; }

        public virtual Route? Route { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
