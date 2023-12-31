﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastGuard.Data;
using FastGuard.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace FastGuard.Controllers
{
	[Authorize(Roles = "Admin, Employee")]

	public class LocationsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public LocationsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Locations
		public async Task<IActionResult> Index(string searchbarinput = "")
		{
			DateTime parsedDate;
			bool isDate = DateTime.TryParse(searchbarinput, out parsedDate);

			var applicationDbContext = _context.Locations;


			var locations = applicationDbContext
				.Where(r => r.LocationName.Contains(searchbarinput));

			if (searchbarinput == null)
				return View(await applicationDbContext.ToListAsync());
			else
				return View(locations);
		}

		// GET: Locations/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Locations == null)
			{
				return NotFound();
			}

			var location = await _context.Locations
				.FirstOrDefaultAsync(m => m.LocationId == id);
			if (location == null)
			{
				return NotFound();
			}

			return View(location);
		}

		// GET: Locations/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Locations/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		// POST: Locations/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("LocationId,LocationName")] Location location)
		{
			if (ModelState.IsValid)
			{
				// Check if the location already exists
				bool locationExists = await _context.Locations.AnyAsync(l => l.LocationName == location.LocationName);
				if (locationExists)
				{
					ModelState.AddModelError("LocationName", "Địa điểm đã tồn tại.");
					return View(location);
				}

				_context.Add(location);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(location);
		}

		// GET: Locations/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Locations == null)
			{
				return NotFound();
			}

			var location = await _context.Locations.FindAsync(id);
			if (location == null)
			{
				return NotFound();
			}
			return View(location);
		}

		// POST: Locations/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("LocationId,LocationName")] Location location)
		{
			if (id != location.LocationId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(location);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!LocationExists(location.LocationId))
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
			return View(location);
		}

		// GET: Locations/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Locations == null)
			{
				return NotFound();
			}

			var location = await _context.Locations
				.FirstOrDefaultAsync(m => m.LocationId == id);
			if (location == null)
			{
				return NotFound();
			}

			return View(location);
		}

		// POST: Locations/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Locations == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Locations'  is null.");
			}
			var location = await _context.Locations.FindAsync(id);
			if (location != null)
			{
				_context.Locations.Remove(location);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool LocationExists(int id)
		{
			return (_context.Locations?.Any(e => e.LocationId == id)).GetValueOrDefault();
		}
	}
}
