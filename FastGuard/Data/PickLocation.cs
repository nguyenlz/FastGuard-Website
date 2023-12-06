using System;
using System.Collections.Generic;

namespace FastGuard.Data
{
    public partial class PickLocation
    {
        public PickLocation()
        {
            InvoicePickLocationId1Navigations = new HashSet<Invoice>();
            InvoicePickLocationId2Navigations = new HashSet<Invoice>();
        }

        public int PickLocationId { get; set; }
        public string? PickLocationName { get; set; }
        public int? LocationId { get; set; }

        public virtual Location? Location { get; set; }
        public virtual ICollection<Invoice> InvoicePickLocationId1Navigations { get; set; }
        public virtual ICollection<Invoice> InvoicePickLocationId2Navigations { get; set; }
    }
}
