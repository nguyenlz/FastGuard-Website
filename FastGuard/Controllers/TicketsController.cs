using FastGuard.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastGuard.Controllers
{
	public class TicketsController : Controller
	{
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> SearchTicket()
		{
            return _context.Locations != null ?
                          View(await _context.Locations.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Locations'  is null.");
        }

		[HttpPost]
		public async Task<IActionResult> Tickets(string start, int startid, string end, int endid, DateTime date)
		{
			ViewData["route"] = end + " id: " + endid;
			return _context.Routes != null ?
						  View(await _context.Routes.ToListAsync()) :
						  Problem("Entity set 'ApplicationDbContext.Routes'  is null.");
		}
	}
}
