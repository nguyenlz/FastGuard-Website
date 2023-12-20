using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastGuard.Controllers
{
    public class RoutesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public RoutesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var applicationDbContext = _context.Routes.Include(r => r.Coach).Include(r => r.LocationId1Navigation).Include(r => r.LocationId2Navigation);
                return View("IndexOP",await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Routes.Include(r => r.Coach).Include(r => r.LocationId1Navigation).Include(r => r.LocationId2Navigation);
                return View(await applicationDbContext.ToListAsync());
            }
                
            
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.Coach)
                .Include(r => r.LocationId1Navigation)
                .Include(r => r.LocationId2Navigation)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Routes/Create
        public IActionResult Create()
        {
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "CoachId");
            ViewData["LocationId1"] = new SelectList(_context.Locations, "LocationId", "LocationName");
            ViewData["LocationId2"] = new SelectList(_context.Locations, "LocationId", "LocationName");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(",CoachId,LocationId1,LocationId2,StartDate,EndDate,Price")] Models.Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "CoachId", route.CoachId);
            ViewData["LocationId1"] = new SelectList(_context.Locations, "LocationId", "LocationId", route.LocationId1);
            ViewData["LocationId2"] = new SelectList(_context.Locations, "LocationId", "LocationId", route.LocationId2);
            return View(route);
        }

        // GET: Routes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "CoachId", route.CoachId);
            ViewData["LocationId1"] = new SelectList(_context.Locations, "LocationId", "LocationId", route.LocationId1);
            ViewData["LocationId2"] = new SelectList(_context.Locations, "LocationId", "LocationId", route.LocationId2);
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteId,CoachId,LocationId1,LocationId2,StartDate,EndDate,Price")] Models.Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.RouteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "CoachId", route.CoachId);
            ViewData["LocationId1"] = new SelectList(_context.Locations, "LocationId", "LocationId", route.LocationId1);
            ViewData["LocationId2"] = new SelectList(_context.Locations, "LocationId", "LocationId", route.LocationId2);
            return View(route);
        }

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.Coach)
                .Include(r => r.LocationId1Navigation)
                .Include(r => r.LocationId2Navigation)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Routes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Routes'  is null.");
            }
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
          return (_context.Routes?.Any(e => e.RouteId == id)).GetValueOrDefault();
        }
    }
}
