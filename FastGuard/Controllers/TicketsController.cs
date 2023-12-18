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
		public IActionResult Tickets(string start, int startid, string end, int endid, DateTime startdate)
		{
			string strStartDate = startdate.ToString("yyyy-MM-dd");
			ViewData["route"] = start + " - " + end;
			ApplicationDbContext context = HttpContext.RequestServices.GetService(typeof(FastGuard.Data.ApplicationDbContext)) as ApplicationDbContext;
			return View(context.SearchRoute(startid, endid, strStartDate));
		}
	}
}
