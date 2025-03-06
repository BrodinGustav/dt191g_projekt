using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dt191g_projekt.Data;
using dt191g_projekt.Models;
using Microsoft.AspNetCore.Authorization;

namespace dt191g_projekt.Controllers
{
    public class SanitationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SanitationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sanitation
        public async Task<IActionResult> Index()
        {
            //Kontroll om _context.Sanitations är null
            if (_context.Sanitations == null)
            {
                return NotFound();
            }

            var sanitations = _context.Sanitations
            .Include(s => s.Customer)
            .Include(s => s.Worker);

            return View(await _context.Sanitations.ToListAsync());
        }


        // GET: Sanitation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_context.Sanitations == null)
            {
                return NotFound();
            }

            var sanitationModel = await _context.Sanitations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanitationModel == null)
            {
                return NotFound();
            }

            return View(sanitationModel);
        }

        [Authorize]
        // GET: Sanitation/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Name");
            return View();
        }

        // POST: Sanitation/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanitationModel sanitation)       {
            if (ModelState.IsValid)
            {
                _context.Add(sanitation);

                //Lägg till logged in user till created by
                sanitation.CreatedBy = User.Identity?.Name ?? "Unknown";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
             ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", sanitation.CustomerId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Name", sanitation.WorkerId);
            return View(sanitation);
        }

        [Authorize]
        // GET: Sanitation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_context.Sanitations == null)
            {
                return NotFound();
            }

            var sanitationModel = await _context.Sanitations.FindAsync(id);
            if (sanitationModel == null)
            {
                return NotFound();
            }
            return View(sanitationModel);
        }

        // POST: Sanitation/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SanitationType,Location,Description,WasteAmount")] SanitationModel sanitationModel)
        {
            if (id != sanitationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanitationModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanitationModelExists(sanitationModel.Id))
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
            return View(sanitationModel);
        }

        [Authorize]
        // GET: Sanitation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_context.Sanitations == null)
            {
                return NotFound();
            }

            var sanitationModel = await _context.Sanitations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanitationModel == null)
            {
                return NotFound();
            }

            return View(sanitationModel);
        }

        // POST: Sanitation/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sanitations == null)
            {
                return NotFound();
            }

            var sanitationModel = await _context.Sanitations.FindAsync(id);
            if (sanitationModel != null)
            {
                _context.Sanitations.Remove(sanitationModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanitationModelExists(int id)
        {
            if (_context.Sanitations == null)
            {
                return false;
            }
            return _context.Sanitations.Any(e => e.Id == id);
        }
    }
}
