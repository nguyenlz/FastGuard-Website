using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FastGuard.Controllers
{
	[Authorize(Roles = "Admin")]
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

		public async Task<IActionResult> Index(string searchbarinput = "")
		{
			DateTime searchDate;
			bool isDate = DateTime.TryParse(searchbarinput, out searchDate);

			var users = await _userManger.GetUsersInRoleAsync("Employee");
			var users2 = users.Where(u => u.Name != null && u.Name.Contains(searchbarinput)
									|| u.Email != null && u.Email.Contains(searchbarinput)
									|| u.PhoneNumber != null && u.PhoneNumber.Contains(searchbarinput)
									|| u.salary != null && u.salary.ToString().Contains(searchbarinput)
									|| (DateTime.TryParseExact(searchbarinput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)
									 && u.DateOfBirth.Date == parsedDate.Date))
							  .ToList();

			return View(users2);

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
                 "Name,salary")] ApplicationUser employee)
        {
            ViewData["ErrorCode"] = 1;
            bool check = _context.checkExistUser(employee.Email);
            if (check)
            {
                ViewData["ErrorCode"] = 0;
                return View("Create");
            }
            else
            {
                employee.UserName = employee.Email;
                if (await _roleManager.RoleExistsAsync("Employee"))
                {

                    _context.Add(employee);
                    var result = await _userManger.AddPasswordAsync(employee, employee.PasswordHash);
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    employee.PasswordHash = passwordHasher.HashPassword(employee, employee.PasswordHash);
                    await _context.SaveChangesAsync();
                    await _userManger.AddToRoleAsync(employee, "Employee");

                }
                return RedirectToAction(nameof(Index));
            }
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
            ViewData["EditCodeEdit"] = 1;
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
        "Name,salary")] ApplicationUser employee)
        {
            employee.UserName = employee.Email;
            ViewData["EditCodeEdit"] = 1;

            bool check = _context.checkExistUserEdit(employee.Id, employee.Email);
            if (check)
            {
                ViewData["EditCodeEdit"] = 0;
                return View("Edit", employee);
            }
            else
            {
                if (id != employee.Id)
                {
                    return NotFound();
                }

                var existingUser = await _userManger.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == employee.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                if (existingUser.ConcurrencyStamp != employee.ConcurrencyStamp)
                {

                    employee.ConcurrencyStamp = existingUser.ConcurrencyStamp;
                }
                else
                {
                    // Không có xung đột, cập nhật giá trị ConcurrencyStamp mới
                    employee.ConcurrencyStamp = Guid.NewGuid().ToString();
                }
                // Kiểm tra xem có đối tượng nào khác đang được theo dõi trong DbContext có cùng Id như driver không
                var trackedUser = _context.Set<ApplicationUser>().Local.SingleOrDefault(u => u.Id == employee.Id);
                if (trackedUser != null)
                {
                    _context.Entry(trackedUser).State = EntityState.Detached;
                }

                try
                {
                    var newPass = employee.PasswordHash;
                    var CurrentDriver = await _userManger.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == employee.Id);
                    if (CurrentDriver != null)
                    employee.PasswordHash = CurrentDriver.PasswordHash;
                    _context.Update(employee);
                    await _userManger.UpdateAsync(employee);
                    var token = await _userManger.GeneratePasswordResetTokenAsync(employee);
                    var result = await _userManger.ResetPasswordAsync(employee, token, newPass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                    return View(employee);
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
