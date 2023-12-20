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
		public float TotalMoney { get; set; }

		public Ticket()
        {
            UserId = string.Empty;
            SeatNo = string.Empty;
            InvoiceDate = DateTime.MinValue;
            PickLocationId1 = 0;
            PickLocationId2 = 0;
            RouteId = 0;
            TotalMoney = 0;
		}
		public Ticket(string userid, string seatno, int pick1, int pick2, int routeid, float total)
		{
			UserId = userid;
			SeatNo = seatno;
			PickLocationId1 = pick1;
			PickLocationId2 = pick2;
			RouteId = routeid;
			TotalMoney = total;
		}
		public virtual PickLocation PickLocationId1Navigation { get; set; }
        public virtual PickLocation PickLocationId2Navigation { get; set; }
        public virtual Route Route { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
