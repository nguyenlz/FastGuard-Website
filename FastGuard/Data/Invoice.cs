using System;
using System.Collections.Generic;

namespace FastGuard.Data
{
    public partial class Invoice
    {
        public int InvoiceId { get; set; }
        public int? TicketId { get; set; }
        public int? UserId { get; set; }
        public int? SeatNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? PickLocationId1 { get; set; }
        public int? PickLocationId2 { get; set; }

        public virtual PickLocation? PickLocationId1Navigation { get; set; }
        public virtual PickLocation? PickLocationId2Navigation { get; set; }
        public virtual Ticket? Ticket { get; set; }
        public virtual User? User { get; set; }
    }
}
