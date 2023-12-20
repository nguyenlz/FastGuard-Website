﻿using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastGuard.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IUserStore<ApplicationUser> _userStore;

        public CustomersController(UserManager<ApplicationUser> userManger,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserStore<ApplicationUser> userStore,

            ApplicationDbContext context)
        {
            _userManger = userManger;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailStore = GetEmailStore();
            _userStore = userStore;

        }

        public IActionResult Index()
        {

            var users = _userManger.GetUsersInRoleAsync("Customer").Result;
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> GetCustomerInfo(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);

            if (user != null)
            {
                // Người dùng được tìm thấy
                // Thực hiện các thao tác khác với người dùng tại đây
                // Ví dụ: Trả về thông tin người dùng dưới dạng JSON
                return Json(new { success = true, name = user.Name, phone = user.PhoneNumber });
            }
            else
            {
                // Người dùng không được tìm thấy
                // Xử lý tình huống không tìm thấy người dùng tại đây
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserName, NormalizedUserName, Email, " +
            "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
            "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd,LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
            "Name")] ApplicationUser customer)
        {
            if (customer != null)
            {
                var result = await _userManger.CreateAsync(customer, customer.PasswordHash);
            }
            customer.UserName = customer.Email;
            if (await _roleManager.RoleExistsAsync("Customer"))
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                await _userManger.AddToRoleAsync(customer, "Customer");
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

            var customer = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customer'  is null.");
            }
            var customer = await _context.Users.FindAsync(id);
            if (customer != null)
            {
                _context.Users.Remove(customer);
                await _context.SaveChangesAsync();
                await _userManger.RemoveFromRoleAsync(customer, "Customer");

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

            var customer = await _context.Users.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", customer.Id);
            return View(customer);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, UserName, NormalizedUserName, Email, " +
    "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
    "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd,LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
    "Name")] ApplicationUser customer)
        {
            customer.UserName = customer.Email;

            if (id != customer.Id)
            {
                return NotFound();
            }

            var existingUser = await _userManger.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.ConcurrencyStamp != customer.ConcurrencyStamp)
            {
                // Xung đột xảy ra, giữ lại giá trị ConcurrencyStamp của người dùng trong cơ sở dữ liệu
                customer.ConcurrencyStamp = existingUser.ConcurrencyStamp;
            }
            else
            {
                // Không có xung đột, cập nhật giá trị ConcurrencyStamp mới
                customer.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            // Kiểm tra xem có đối tượng nào khác đang được theo dõi trong DbContext có cùng Id như employee không
            var trackedUser = _context.Set<ApplicationUser>().Local.SingleOrDefault(u => u.Id == customer.Id);
            if (trackedUser != null)
            {
                _context.Entry(trackedUser).State = EntityState.Detached;
            }

            try
            {
                _context.Update(customer);

                var token = await _userManger.GeneratePasswordResetTokenAsync(customer);

                var result = await _userManger.ResetPasswordAsync(customer, token, customer.PasswordHash);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Xử lý xung đột xảy ra trong quá trình cập nhật
                // Thực hiện các bước khác để xử lý xung đột
                // Ví dụ: Truy vấn lại dữ liệu và thử lại quá trình cập nhật
                ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                return View(customer);
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

            var customer = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManger.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
