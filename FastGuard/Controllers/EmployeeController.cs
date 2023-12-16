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

public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public EmployeeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _context = context;
    }

    public IActionResult Index()
    {
        if (User.IsInRole("Admin"))
        {
            var allUsers = _context.Users.ToList();
            return View("IndexOP",allUsers);
        }
        else
        {

            var allUsers = _context.Users.ToList();
            return View("Index", allUsers);
        }
    }

   

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id, UserName, NormalizedUserName, Email, " +
        "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
        "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
        "Name")] ApplicationUser user)
    {
        if (ModelState.IsValid)
        {
            await _userManager.CreateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            // Retrieve and delete associated records in the coaches table
            var associatedCoaches = _context.Coaches.Where(c => c.UserId == id).ToList();
            _context.Coaches.RemoveRange(associatedCoaches);

            // Delete the user
            await _userManager.DeleteAsync(user);
        }

        return RedirectToAction(nameof(Index));
    }

    private bool UsersExists(string id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id, UserName, NormalizedUserName, Email, " +
    "NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, " +
    "PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, DateOfBirth, Discriminator, " +
    "Name")] ApplicationUser updatedUser)
    {
        if (id != updatedUser.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                
                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.Name = updatedUser.Name;
                existingUser.PasswordHash = _userManager.PasswordHasher.HashPassword(existingUser, updatedUser.PasswordHash);
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.DateOfBirth = updatedUser.DateOfBirth;

                
                await _userManager.UpdateAsync(existingUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(updatedUser.Id))
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

        return View(updatedUser);
    }
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
}
