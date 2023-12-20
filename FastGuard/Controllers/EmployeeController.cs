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
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public EmployeeController(UserManager<ApplicationUser> userManger, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManger = userManger;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_userManger.GetUsersInRoleAsync("Employee").Result);

        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserName, NormalizedUserName, Email, " +
        "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
            "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd,LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
            "Name")] ApplicationUser employee)
        {
            if (employee != null)
            {
                var result = await _userManger.CreateAsync(employee, employee.PasswordHash);
            }
            employee.UserName = employee.Email;
            if (await _roleManager.RoleExistsAsync("Employee"))
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                await _userManger.AddToRoleAsync(employee, "Employee");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Driver/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var employee = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.employee'  is null.");
            }
            var employee = await _context.Users.FindAsync(id);
            if (employee != null)
            {
                _context.Users.Remove(employee);
                await _context.SaveChangesAsync();
                await _userManger.RemoveFromRoleAsync(employee, "Employee");

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

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var employee = await _context.Users.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", employee.Id);
            return View(employee);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, UserName, NormalizedUserName, Email, " +
    "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
    "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd,LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
    "Name")] ApplicationUser employee)
        {
            employee.UserName = employee.Email;

            if (id != employee.Id)
            {
                return NotFound();
            }

            var existingUser = await _userManger.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.ConcurrencyStamp != employee.ConcurrencyStamp)
            {
                // Xung đột xảy ra, giữ lại giá trị ConcurrencyStamp của người dùng trong cơ sở dữ liệu
                employee.ConcurrencyStamp = existingUser.ConcurrencyStamp;
            }
            else
            {
                // Không có xung đột, cập nhật giá trị ConcurrencyStamp mới
                employee.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            // Kiểm tra xem có đối tượng nào khác đang được theo dõi trong DbContext có cùng Id như employee không
            var trackedUser = _context.Set<ApplicationUser>().Local.SingleOrDefault(u => u.Id == employee.Id);
            if (trackedUser != null)
            {
                _context.Entry(trackedUser).State = EntityState.Detached;
            }

            try
            {
                _context.Update(employee);

                var token = await _userManger.GeneratePasswordResetTokenAsync(employee);

                var result = await _userManger.ResetPasswordAsync(employee, token, employee.PasswordHash);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Xử lý xung đột xảy ra trong quá trình cập nhật
                // Thực hiện các bước khác để xử lý xung đột
                // Ví dụ: Truy vấn lại dữ liệu và thử lại quá trình cập nhật
                ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                return View(employee);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Driver/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var employee = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
    }
}
