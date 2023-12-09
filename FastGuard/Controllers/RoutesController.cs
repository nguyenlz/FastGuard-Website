using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastGuard.Data;

namespace FastGuard.Controllers
{
    public class RoutesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Routes.Include(r => r.Coach).Include(r => r.LocationId1Navigation).Include(r => r.LocationId2Navigation);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["LocationId1"] = new SelectList(_context.Locations, "LocationId", "LocationId");
            ViewData["LocationId2"] = new SelectList(_context.Locations, "LocationId", "LocationId");
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
