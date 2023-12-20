using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace FastGuard.Controllers
{
	[Authorize]
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
		public async Task<IActionResult> BookedTicket()
		{
			var user = await _userManager.GetUserAsync(User);

			List<Ticket> bookedTickets = new List<Ticket>();

			if (await _userManager.IsInRoleAsync(user, "Customer"))
			{
				// Lấy danh sách vé đã đặt của khách hàng
				bookedTickets = await _context.Tickets
					.Where(t => t.UserId == user.Id)
					.ToListAsync();
			}
			else if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Employee"))
			{
				// Lấy tất cả danh sách vé cho admin
				bookedTickets = await _context.Tickets.ToListAsync();
			}

			return View(bookedTickets);
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
			//lay ve da dat theo routeid
			var tickets = (from a in _context.Tickets
						   where a.RouteId == routeid
						   select a).ToList();

			ViewData["SeatNo"] = new SelectList(tickets, "SeatNo", "SeatNo");
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
		public async Task<IActionResult> PayResult(string cusemail, string cusphone, string cusname, string[] selectedSeats, int startid, int endid, int routeid, float price)
		{
			var user = await _userManager.FindByEmailAsync(cusemail);
			if (user != null)
			{
				user.Name = cusname;
				user.PhoneNumber = cusphone;
				await _userManager.UpdateAsync(user);					
			}
			else
			{
				ApplicationUser newUser = new ApplicationUser
				{
					UserName = cusemail,
					Email = cusemail,
					Name = cusname,
					PhoneNumber = cusphone
				};

				string password = "Customer1.";

				var passwordHasher = new PasswordHasher<ApplicationUser>();
				newUser.PasswordHash = passwordHasher.HashPassword(newUser, password);

				var result = await _userManager.CreateAsync(newUser);

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(newUser, "Customer");
					user = newUser;
				}
				else
				{
					ViewData["message"] = "Tạo người dùng mới thất bại";
					return View();
				}
			}
			foreach (string seat in selectedSeats)
			{
				if (FindTicketBySeatNoAndRouteId(seat, routeid) == null)
				{
					Ticket ticket = new Ticket(user.Id, seat, startid, endid, routeid, price);
					int count = _context.CreateTicket(ticket);
					if (count <= 0)
					{
						ViewData["message"] = "Đặt vé thất bại";
						return View();
					}
				}

			}
			ViewData["message"] = "Đặt vé thành công";
			return View();
		}
        public Ticket FindTicketBySeatNoAndRouteId(string seatNo, int routeId)
        {

            Ticket ticket = _context.Tickets
                .FirstOrDefault(t => t.SeatNo == seatNo && t.RouteId == routeId);

            return ticket;
        }
    }
}
