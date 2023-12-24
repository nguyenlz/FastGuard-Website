using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Điểm đi không được để trống")]
        [Display(Name = "Điểm đi")]
        public int? LocationId1 { get; set; }

        [Required(ErrorMessage = "Điểm đón không được để trống")]
        [Display(Name = "Điểm đón")]
        public int? LocationId2 { get; set; }

		[Required(ErrorMessage = "Ngày khởi hành không được để trống")]
		[Display(Name = "Ngày khởi hành")]
		public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày đến không được để trống")]
        [Display(Name = "Ngày đến")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Giá vé không được để trống")]
        [Display(Name = "Giá vé")]
        public float? Price { get; set; }

		public virtual Coach? Coach { get; set; }
        public virtual Location? LocationId1Navigation { get; set; }
        
        public virtual Location? LocationId2Navigation { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
