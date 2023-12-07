using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastGuard.Data;
using FastGuard.Models;

namespace FastGuard.Controllers
{
    public class PickLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PickLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PickLocations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PickLocations.Include(p => p.Location);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PickLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PickLocations == null)
            {
                return NotFound();
            }

            var pickLocation = await _context.PickLocations
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.PickLocationId == id);
            if (pickLocation == null)
            {
                return NotFound();
            }

            return View(pickLocation);
        }

        // GET: PickLocations/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
            return View();
        }

        // POST: PickLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PickLocationId,PickLocationName,LocationId")] PickLocation pickLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pickLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", pickLocation.LocationId);
            return View(pickLocation);
        }

        // GET: PickLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PickLocations == null)
            {
                return NotFound();
            }

            var pickLocation = await _context.PickLocations.FindAsync(id);
            if (pickLocation == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", pickLocation.LocationId);
            return View(pickLocation);
        }

        // POST: PickLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PickLocationId,PickLocationName,LocationId")] PickLocation pickLocation)
        {
            if (id != pickLocation.PickLocationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pickLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PickLocationExists(pickLocation.PickLocationId))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", pickLocation.LocationId);
            return View(pickLocation);
        }

        // GET: PickLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PickLocations == null)
            {
                return NotFound();
            }

            var pickLocation = await _context.PickLocations
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.PickLocationId == id);
            if (pickLocation == null)
            {
                return NotFound();
            }

            return View(pickLocation);
        }

        // POST: PickLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PickLocations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PickLocations'  is null.");
            }
            var pickLocation = await _context.PickLocations.FindAsync(id);
            if (pickLocation != null)
            {
                _context.PickLocations.Remove(pickLocation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PickLocationExists(int id)
        {
          return (_context.PickLocations?.Any(e => e.PickLocationId == id)).GetValueOrDefault();
        }
    }
}
