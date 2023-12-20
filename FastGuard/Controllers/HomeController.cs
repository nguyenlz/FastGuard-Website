using FastGuard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FastGuard.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace FastGuard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            List<int> list = new List<int>();
            // lấy số lượng nhân viên 
            var listEmployeeTask = _userManager.GetUsersInRoleAsync("Employee");
            listEmployeeTask.Wait(); // Đợi kết quả trả về
            var listEmployee = listEmployeeTask.Result;
            list.Add( listEmployee.Count);

            //lấy số lượng khách hàng 
            var listCustomerTask = _userManager.GetUsersInRoleAsync("Customer");
            listCustomerTask.Wait(); // Đợi kết quả trả về
            var listCustomer = listCustomerTask.Result;
            list.Add(listCustomer.Count);

            // lấy số lượng tài xế 
            var listDriverTask = _userManager.GetUsersInRoleAsync("Driver");
            listDriverTask.Wait(); // Đợi kết quả trả về
            var listDriver = listDriverTask.Result;
            list.Add(listDriver.Count);

            // lấy số lượng xe 
            list.Add(_context.countCoach());

            // lay so luong diem doan (pick location)
            list.Add(_context.countPickLocation());

            // lay so luong dia diem 
            list.Add(_context.countLocation());

            // lay so tuyen (routes)
            list.Add(_context.countRoutes());

            // lay so luong ve
            list.Add(_context.countTicket());

            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}