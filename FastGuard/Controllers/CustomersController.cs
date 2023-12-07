using FastGuard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FastGuard.Controllers
{
    public class CustomersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public CustomersController(UserManager<ApplicationUser> userManger, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManger = userManger;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var users = _userManger.GetUsersInRoleAsync("Customer").Result;
            return View(users);
        }
    }
}
