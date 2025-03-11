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
            .Include(s => s.Worker)
            .ToListAsync();

            return View(await sanitations);
        }


        // GET: Sanitation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context?.Sanitations == null)
            {
                return NotFound();
            }

            var sanitation = await _context.Sanitations


                   .Include(s => s.Worker)  // Inkluderar Worker
                   .Include(s => s.Customer) // Inkluderar Customer
                   .FirstOrDefaultAsync(m => m.Id == id);

            if (_context.Sanitations == null)
            {
                return NotFound();
            }

            return View(sanitation);
        }

        [Authorize]
        // GET: Sanitation/Create
        public IActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "Name");
            ViewBag.WorkerId = new SelectList(_context.Workers, "Id", "Name");
            return View();
        }

        // POST: Sanitation/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanitationModel sanitation)
        {

            if (_context?.Sanitations == null)
            {
                return NotFound();
            }

            //Kontroll om en post med samma SanitationType, Location och Description redan finns 
            var existingSanitation = await _context.Sanitations
                .FirstOrDefaultAsync(s => s.Location == sanitation.Location
                                          && s.Description == sanitation.Description);

            //Om post finns, visa felmeddelande
            if (existingSanitation != null)
            {
                ModelState.AddModelError(string.Empty, "En liknande order existerar redan.");
            }

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
            if (id == null || _context?.Sanitations == null)
            {
                return NotFound();
            }

            var sanitation = await _context.Sanitations
       .Include(s => s.Worker)   // Laddar Worker
       .Include(s => s.Customer) // Laddar Customer
       .FirstOrDefaultAsync(m => m.Id == id);

            if (sanitation == null)
            {
                return NotFound();
            }

            //Skickar lista på Workers och Customer för dropdown-meny
            ViewBag.WorkerId = new SelectList(_context.Workers, "Id", "Name", sanitation.WorkerId);
            ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "Name", sanitation.CustomerId);
            return View(sanitation);
        }

        // POST: Sanitation/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SanitationType,Location,Description,WasteAmount,WorkerId,CustomerId")] SanitationModel sanitationModel)
        {
            if (id != sanitationModel.Id || _context?.Sanitations == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                //Hämtar befintliga posten från databasen för att bevara CreatedBy
                var existingSanitation = await _context.Sanitations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (existingSanitation == null)
                {
                    return NotFound();
                }

                //Kontroll om post med samma SanitationType, Location och Description finns
                var duplicatedSanitation = await _context.Sanitations
                    .Where(s => s.Location == sanitationModel.Location
                            && s.Description == sanitationModel.Description &&
                            s.Id != sanitationModel.Id) //Exkluderar nuvarande posten
                    .FirstOrDefaultAsync();


                if (duplicatedSanitation != null)
                {
                    //Om post finns, skicka felmeddelande i ModelState
                    ModelState.AddModelError("", "En liknande order med samma beskrivning finns redan.");

                    sanitationModel.CreatedBy = existingSanitation.CreatedBy;

                    //Lägger till listor för Worker och Customer för att undvika tomma listor
                    ViewBag.WorkerId = new SelectList(_context.Workers, "Id", "Name", sanitationModel.WorkerId);
                    ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "Name", sanitationModel.CustomerId);



                    //Skicka användare till Edit-vyn och visa felmeddelandet
                    return View(sanitationModel);
                }

                try
                {
                    //Bevara CreatedBy
                    sanitationModel.CreatedBy = existingSanitation.CreatedBy;

                    //Om ok, uppdatera databasen
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


            //Laddar dropdown-menyer igen vid valideringsfel
            ViewBag.WorkerId = new SelectList(_context.Workers, "Id", "Name", sanitationModel.WorkerId);
            ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "Name", sanitationModel.CustomerId);

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
