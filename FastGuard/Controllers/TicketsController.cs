using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace FastGuard.Controllers
{
	public class TicketsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		public TicketsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public async Task<IActionResult> SearchTicket()
		{
			return _context.Locations != null ?
						  View(await _context.Locations.ToListAsync()) :
						  Problem("Entity set 'ApplicationDbContext.Locations'  is null.");
		}

		[HttpGet]
		public IActionResult Tickets(string start, int startid, string end, int endid, DateTime startdate)
		{
			string strStartDate = startdate.ToString("yyyy-MM-dd");
			ViewData["route"] = start + " - " + end;
			ApplicationDbContext context = HttpContext.RequestServices.GetService(typeof(FastGuard.Data.ApplicationDbContext)) as ApplicationDbContext;
			List<Models.Route> list = context.SearchRoute(startid, endid, strStartDate);
			return View(list);
		}

		public async Task<IActionResult> Checkout(int routeid, string start, string end)
		{
			ViewData["start"] = start;
			ViewData["end"] = end;
			//lay pick location 1
			var query = from a in _context.Routes
						join b in _context.Locations on a.LocationId1 equals b.LocationId
						join c in _context.PickLocations on b.LocationId equals c.LocationId
						where a.RouteId == routeid
						select new { Route = a, Location = b, PickLocation = c };

			var picklocations1 = query.ToList();
			ViewData["PickLocationId1"] = new SelectList(picklocations1, "PickLocation.PickLocationId", "PickLocation.PickLocationName");
			//lay pick location 2
			query = from a in _context.Routes
					join b in _context.Locations on a.LocationId2 equals b.LocationId
					join c in _context.PickLocations on b.LocationId equals c.LocationId
					where a.RouteId == routeid
					select new { Route = a, Location = b, PickLocation = c };

			var picklocations2 = query.ToList();
			ViewData["PickLocationId2"] = new SelectList(picklocations2, "PickLocation.PickLocationId", "PickLocation.PickLocationName");

			//lay thong tin signed in user
			if (_signInManager.IsSignedIn(User))
			{
				var userId = _userManager.GetUserId(User);
				var user = _userManager.FindByIdAsync(userId).Result;

				ViewBag.name = user.Name;
				ViewBag.email = user.Email;
				ViewBag.phone = user.PhoneNumber;
			}

			//lay route theo id
			var route = await _context.Routes
				.FirstOrDefaultAsync(m => m.RouteId == routeid);
			return View(route);
		}
		[HttpGet]
		public async Task<IActionResult> PayResult(string cusemail, string cusphone, string cusname)
		{
			var user = await _userManager.FindByEmailAsync(cusemail);
			if (user != null)
			{
				user.Name = cusname;
				user.PhoneNumber = cusphone;
				await _userManager.UpdateAsync(user);
			}

			return View();
		}
	}
}
