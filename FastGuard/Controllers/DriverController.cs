using FastGuard.Areas.Identity.Pages.Account;
using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;


namespace FastGuard.Controllers
{
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public DriverController(UserManager<ApplicationUser> userManger, RoleManager<IdentityRole> roleManager
            , IConfiguration configuration, ApplicationDbContext context)
        {
            _userManger = userManger;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index()
        {
			var drivers = _userManger.GetUsersInRoleAsync("Driver").Result;

            return View(drivers);

        }

        public IActionResult Create()
        {
            ViewData["ErrorCode"] = 1; 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserName, NormalizedUserName, Email, " +
                 "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
                 "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd,LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
                 "Name,salary")] ApplicationUser driver)
        {
            ViewData["ErrorCode"] = 1;
            bool check = _context.checkExistUser(driver.Email);
            if (check)
            {
                ViewData["ErrorCode"] = 0;
                return View("Create");
            }
            else
            {
                driver.UserName = driver.Email;
                if (await _roleManager.RoleExistsAsync("Driver"))
                {
                  
                    _context.Add(driver);
                    var result = await _userManger.AddPasswordAsync(driver, driver.PasswordHash);
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    driver.PasswordHash = passwordHasher.HashPassword(driver, driver.PasswordHash);
                    await _context.SaveChangesAsync();
                    await _userManger.AddToRoleAsync(driver, "Driver");

                }
                return RedirectToAction(nameof(Index));
            }
        }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(driver);
        }


        // GET: Driver/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var driver = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var driver = await _context.Users.FindAsync(id);
            
            bool check = _context.checkDriverWithCoach(id);
            ViewData["ErrorDelete"] = "";
            if (check)
            {
                ViewData["ErrorDelete"] = "Không thể xóa tài xế, tài xế đang phụ trách xe";
                return View("Delete",driver);
            }


            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Driver' is null.");
            }
           
            if (driver != null)
            {
                // Check if the driver is associated with any coaches
                var isDriverAssociatedWithCoaches = _context.Coaches.Any(c => c.UserId == id);

                if (isDriverAssociatedWithCoaches)
                {
                    TempData["Notice"] = "Cannot delete the driver. Please remove associated coaches first.";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userManger.DeleteAsync(driver);

                if (result.Succeeded)
                {
                    await _userManger.RemoveFromRoleAsync(driver, "Driver");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Coaches/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["EditCodeEdit"] = 1;
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var driver = await _context.Users.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", driver.Id);
            return View(driver);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, UserName, NormalizedUserName, Email, " +
        "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
        "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd,LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
        "Name,salary")] ApplicationUser driver)
        {
            driver.UserName = driver.Email;
            ViewData["EditCodeEdit"] = 1;

            bool check = _context.checkExistUserEdit(driver.Id, driver.Email);
            if (check)
            {
                ViewData["EditCodeEdit"] = 0;
                return View("Edit", driver);
            }
            else
            {
                if (id != driver.Id)
                {
                    return NotFound();
                }

                var existingUser = await _userManger.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == driver.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                if (existingUser.ConcurrencyStamp != driver.ConcurrencyStamp)
                {

                    driver.ConcurrencyStamp = existingUser.ConcurrencyStamp;
                }
                else
                {
                    // Không có xung đột, cập nhật giá trị ConcurrencyStamp mới
                    driver.ConcurrencyStamp = Guid.NewGuid().ToString();
                }
                // Kiểm tra xem có đối tượng nào khác đang được theo dõi trong DbContext có cùng Id như driver không
                var trackedUser = _context.Set<ApplicationUser>().Local.SingleOrDefault(u => u.Id == driver.Id);
                if (trackedUser != null)
                {
                    _context.Entry(trackedUser).State = EntityState.Detached;
                }

                try
                {
                    var newPass = driver.PasswordHash;
                    var CurrentDriver = await _userManger.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == driver.Id);
                    if (CurrentDriver != null)
                        driver.PasswordHash = CurrentDriver.PasswordHash;
                    _context.Update(driver);
                    await _userManger.UpdateAsync(driver);
                    var token = await _userManger.GeneratePasswordResetTokenAsync(driver);
                    var result = await _userManger.ResetPasswordAsync(driver, token, newPass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                    return View(driver);
                }
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: Driver/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var driver = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

    }
}