using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FastGuard.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
	public class CoachesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoachesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Coaches
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Coaches.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Coaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Coaches == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CoachId == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // GET: Coaches/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["LocationName1"] = new SelectList(_context.Locations, "LocationId", "LocationName");
            ViewData["ErrorCoach"] = "";
            return View();
            //ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            //int count;
            //ApplicationDbContext context = HttpContext.RequestServices.GetService(typeof(FastGuard.Data.ApplicationDbContext)) as ApplicationDbContext;
            //count = context.CreateCoach(c);
            //if (count > 0)
            //    ViewData["thongbao"] = "Insert thành công";
            //else
            //    ViewData["thongbao"] = "Insert không thành công";
            //return View();
        }

        // POST: Coaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoachId,CoachNo,UserId,Supplier,Capacity,Status,Description")] Coach coach)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(coach);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", coach.UserId);
            //return View(coach);
            ViewData["ErrorCoach"] = "";
            var checkCoach = await _context.Coaches
              .FirstOrDefaultAsync(m => m.CoachNo == coach.CoachNo);
            if(checkCoach!=null)
            {
                ViewData["ErrorCoach"] = "Không thể thêm xe, biển số xe đã bị trùng";
                return View("Create");
            }
            _context.Add(coach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Coaches/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Coaches == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", coach.UserId);
            return View(coach);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CoachId,CoachNo,UserId,Supplier,Capacity,Status,Description")] Coach coach)
        {
            if (id != coach.CoachId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(coach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachExists(coach.CoachId))
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
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", coach.UserId);
            return View(coach);
        }

        // GET: Coaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Coaches == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CoachId == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Coaches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Coaches'  is null.");
            }
            var coach = await _context.Coaches.FindAsync(id);
            if (coach != null)
            {
                _context.Coaches.Remove(coach);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoachExists(int id)
        {
          return (_context.Coaches?.Any(e => e.CoachId == id)).GetValueOrDefault();
        }
    }
}
