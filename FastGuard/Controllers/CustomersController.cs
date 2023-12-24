using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FastGuard.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
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
            _context= context;
			_emailStore = GetEmailStore();
			_userStore = userStore;

		}

		public async Task<IActionResult> Index(string searchbarinput = "")
		{
			DateTime searchDate;
			bool isDate = DateTime.TryParse(searchbarinput, out searchDate);

			var users = await _userManger.GetUsersInRoleAsync("Customer");
			var users2 = users.Where(u => u.Name != null && u.Name.Contains(searchbarinput)
									|| u.Email != null && u.Email.Contains(searchbarinput)
									|| u.PhoneNumber != null && u.PhoneNumber.Contains(searchbarinput)
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
            "Name,salary")] ApplicationUser customer)
        {
            if (customer != null)
              //  customer.PasswordHash
            {
                var result = await _userManger.CreateAsync(customer, customer.PasswordHash);
            }
            customer.UserName = customer.Email;
            customer.salary = 0;
            ViewData["ErrorCode"] = 1;
            bool check = _context.checkExistUser(customer.Email);
            if (check)
            {
                ViewData["ErrorCode"] = 0;
                return View("Create");
            }
            else
            {
                if (await _roleManager.RoleExistsAsync("Customer"))
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    await _userManger.AddToRoleAsync(customer, "Customer");
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
            bool check = _context.checkCustomerWithTicket(id);
            var customer = await _context.Users.FindAsync(id);
            ViewData["ErrorDelete"] = "";
            if (check)
            {
                ViewData["ErrorDelete"] = "Không thể xóa khách hàng, khách hàng này đang trong chuyến đi";
                return View("Delete", customer);
            }
            

            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customer'  is null.");
            }
          
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
            ViewData["EditCodeEdit"] = 1;
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
        "Name,salary")] ApplicationUser customer)
        {
            customer.UserName = customer.Email;
            customer.salary = 0;
            ViewData["EditCodeEdit"] = 1;

            bool check = _context.checkExistUserEdit(customer.Id, customer.Email);
            if (check)
            {
                ViewData["EditCodeEdit"] = 0;
                return View("Edit", customer);
            }
            else
            {
                if (id != customer.Id)
                {
                    return NotFound();
                }

                var existingUser = await _userManger.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == customer.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                if (existingUser.ConcurrencyStamp != customer.ConcurrencyStamp)
                {

                    customer.ConcurrencyStamp = existingUser.ConcurrencyStamp;
                }
                else
                {
                    // Không có xung đột, cập nhật giá trị ConcurrencyStamp mới
                    customer.ConcurrencyStamp = Guid.NewGuid().ToString();
                }
                // Kiểm tra xem có đối tượng nào khác đang được theo dõi trong DbContext có cùng Id như driver không
                var trackedUser = _context.Set<ApplicationUser>().Local.SingleOrDefault(u => u.Id == customer.Id);
                if (trackedUser != null)
                {
                    _context.Entry(trackedUser).State = EntityState.Detached;
                }

                try
                {
                    var newPass = customer.PasswordHash;
                    var CurrentDriver = await _userManger.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == customer.Id);
                    if (CurrentDriver != null)
                        customer.PasswordHash = CurrentDriver.PasswordHash;
                    _context.Update(customer);
                    await _userManger.UpdateAsync(customer);
                    var token = await _userManger.GeneratePasswordResetTokenAsync(customer);
                    var result = await _userManger.ResetPasswordAsync(customer, token, newPass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                    return View(customer);
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
