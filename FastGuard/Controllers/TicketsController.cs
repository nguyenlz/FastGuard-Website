using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
        [Authorize(Roles = "Admin, Employee, Customer")]
        public async Task<IActionResult> SearchTicket()
        {
            return _context.Locations != null ?
                          View(await _context.Locations.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Locations'  is null.");
        }
        [Authorize(Roles = "Admin, Employee, Customer")]
        public async Task<IActionResult> BookedTicket(string searchbarinput = "")
        {
            DateTime parsedDate;
            bool isDate = DateTime.TryParse(searchbarinput, out parsedDate);
            TimeSpan parsedTime;
            bool isTime = TimeSpan.TryParseExact(searchbarinput, "HH\\:mm", CultureInfo.InvariantCulture, out parsedTime);
            var user = await _userManager.GetUserAsync(User);

            List<Ticket> bookedTickets = new List<Ticket>();

            if (await _userManager.IsInRoleAsync(user, "Customer"))
            {
                // Lấy danh sách vé đã đặt của khách hàng
                bookedTickets = await _context.Tickets
                    .Where(t => t.UserId == user.Id)
                    .Include(p => p.PickLocationId1Navigation)
                    .Include(p => p.Route)
                    .Include(p => p.PickLocationId2Navigation)
                    .Where(r => r.User.Name.Contains(searchbarinput)
                    || r.SeatNo.Contains(searchbarinput)
                    || r.PickLocationId1Navigation.PickLocationName.Contains(searchbarinput)
                    || r.PickLocationId2Navigation.PickLocationName.Contains(searchbarinput)
                    || r.TotalMoney.ToString().Contains(searchbarinput)
                    || (DateTime.TryParseExact(searchbarinput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        && r.InvoiceDate.Date == parsedDate.Date)
                    || (DateTime.TryParseExact(searchbarinput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        && r.Route.StartDate == parsedDate.Date)
                    || (r.Route.StartDate.TimeOfDay == parsedTime))
                    .ToListAsync();
            }
            else if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Employee"))
            {
                // Lấy tất cả danh sách vé cho admin
                bookedTickets = await _context.Tickets.Include(p => p.User)
                    .Include(p => p.Route)
                    .Include(p => p.PickLocationId1Navigation)
                    .Include(p => p.PickLocationId2Navigation)
                    .Where(r => r.User.Name.Contains(searchbarinput)
                    || r.SeatNo.Contains(searchbarinput)
                    || r.PickLocationId1Navigation.PickLocationName.Contains(searchbarinput)
                    || r.PickLocationId2Navigation.PickLocationName.Contains(searchbarinput)
                    || r.TotalMoney.ToString().Contains(searchbarinput)
                    || r.InvoiceDate.ToString().Contains(searchbarinput)
                    || r.Route.StartDate.ToString().Contains(searchbarinput)
                    || (DateTime.TryParseExact(searchbarinput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        && r.InvoiceDate.Date == parsedDate.Date)
                    || (DateTime.TryParseExact(searchbarinput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        && r.Route.StartDate.Date == parsedDate.Date)
                    || (r.Route.StartDate.TimeOfDay == parsedTime))
                    .ToListAsync();
            }

            return View(bookedTickets);
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            var route = await _context.Routes.FindAsync(ticket.RouteId);

            if (ticket == null)
            {
                return NotFound();
            }
            var seatNos = _context.Seats
                .Where(s => !_context.Tickets
                    .Where(t => t.RouteId == ticket.RouteId)
                    .Any(t => t.SeatNo == s.SeatNo));

            ViewData["SeatNo"] = new SelectList(seatNos, "SeatNo", "SeatNo");

            var pickLocation1 = _context.PickLocations
                .Where(s => s.LocationId == route.LocationId1)
                .ToList();
            ViewData["PickLocationName1"] = new SelectList(pickLocation1, "PickLocationId", "PickLocationName");

            var pickLocation2 = _context.PickLocations
                .Where(s => s.LocationId == route.LocationId2)
                .ToList();
            ViewData["PickLocationName2"] = new SelectList(pickLocation2, "PickLocationId", "PickLocationName");

            return View(ticket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,UserId,SeatNo,InvoiceDate,PickLocationId1,PickLocationId2,RouteId,TotalMoney")] Ticket ticket)
        {
            if (id != ticket.InvoiceId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.InvoiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BookedTicket));
            }

            var route = await _context.Routes.FindAsync(ticket.RouteId);
            var seatNos = _context.Seats
                .Where(s => !_context.Tickets
                    .Where(t => t.RouteId == ticket.RouteId)
                    .Any(t => t.SeatNo == s.SeatNo));

            ViewData["SeatNo"] = new SelectList(seatNos, "SeatNo", "SeatNo");

            var pickLocation1 = _context.PickLocations
                .Where(s => s.LocationId == route.LocationId1)
                .ToList();
            ViewData["PickLocationName1"] = new SelectList(pickLocation1, "PickLocationId", "PickLocationName");

            var pickLocation2 = _context.PickLocations
                .Where(s => s.LocationId == route.LocationId2)
                .ToList();
            ViewData["PickLocationName2"] = new SelectList(pickLocation2, "PickLocationId", "PickLocationName");

            return View(ticket);
        }

        [Authorize(Roles = "Admin, Employee, Customer")]
        [HttpGet]
        public IActionResult Tickets(string start, int startid, string end, int endid, DateTime startdate)
        {
            string strStartDate = startdate.ToString("yyyy-MM-dd");
            ViewData["route"] = start + " - " + end;
            ApplicationDbContext context = HttpContext.RequestServices.GetService(typeof(FastGuard.Data.ApplicationDbContext)) as ApplicationDbContext;
            List<Models.Route> list = context.SearchRoute(startid, endid, strStartDate);
			List<Dictionary<string, object>> routes = context.CountBookedSeat(startid, endid, strStartDate);
			ViewData["routes"] = routes;

			return View(list);
        }

        [Authorize(Roles = "Admin, Employee, Customer")]
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

        [Authorize(Roles = "Admin, Employee, Customer")]
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
                    PhoneNumber = cusphone,
                    DateOfBirth = DateTime.Now,
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

        [Authorize(Roles = "Admin, Employee, Customer")]
        public Ticket FindTicketBySeatNoAndRouteId(string seatNo, int routeId)
        {

            Ticket ticket = _context.Tickets
                .FirstOrDefault(t => t.SeatNo == seatNo && t.RouteId == routeId);

            return ticket;
        }
        [Authorize(Roles = "Admin, Employee, Customer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.PickLocationId1Navigation)
                .Include(t => t.PickLocationId2Navigation)
                .Include(t => t.Route)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            ViewData["start"] = _context.Locations.Where(l => l.LocationId == ticket.Route.LocationId1).Select(l => l.LocationName).FirstOrDefault();
			ViewData["end"] = _context.Locations.Where(l => l.LocationId == ticket.Route.LocationId2).Select(l => l.LocationName).FirstOrDefault();

			if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        [Authorize(Roles = "Admin, Employee, Customer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.PickLocationId1Navigation)
                .Include(t => t.PickLocationId2Navigation)
                .Include(t => t.Route)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookedTicket));
        }
        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
