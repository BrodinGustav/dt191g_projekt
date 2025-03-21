using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanitationApp.Models;
using dt191g_projekt.Data;

namespace dt191g_projekt.Controllers
{
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Worker
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workers.ToListAsync());
        }

        // GET: Worker/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerModel = await _context.Workers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerModel == null)
            {
                return NotFound();
            }

            return View(workerModel);
        }

        // GET: Worker/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Worker/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Role,PhoneNumber")] WorkerModel workerModel)
        {


            if (_context?.Workers == null)
            {
                return NotFound();
            }

            //Kontroll om en post med samma Phonenumber redan finns 
            var existingWorker = await _context.Workers
                .FirstOrDefaultAsync(w => w.PhoneNumber == workerModel.PhoneNumber);


            //Om post finns, visa felmeddelande
            if (existingWorker != null)
            {
                ModelState.AddModelError(string.Empty, "En sanerare med samma telefonnummer finns redan.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(workerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workerModel);
        }

        // GET: Worker/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerModel = await _context.Workers.FindAsync(id);
            if (workerModel == null)
            {
                return NotFound();
            }
            return View(workerModel);
        }

        // POST: Worker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Role,PhoneNumber")] WorkerModel workerModel)
        {
            if (id != workerModel.Id)
            {
                return NotFound();
            }


            //Kontroll om sanerare med samma PhoneNumber finns
            var duplicatedWorker = await _context.Workers
                .Where(w => w.PhoneNumber == workerModel.PhoneNumber &&
                            w.Id != workerModel.Id) //Exkluderar nuvarande posten
                .FirstOrDefaultAsync();


            if (duplicatedWorker != null)
            {
                //Om sanerare finns, skicka felmeddelande i ModelState
                ModelState.AddModelError("", "En sanerare med samma telefonnummer finns redan.");

                //Skicka användare till Edit-vyn och visa felmeddelandet
                return View(workerModel);
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(workerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerModelExists(workerModel.Id))
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
            return View(workerModel);
        }

        // GET: Worker/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workerModel = await _context.Workers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerModel == null)
            {
                return NotFound();
            }

            return View(workerModel);
        }

        // POST: Worker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workerModel = await _context.Workers.FindAsync(id);
            if (workerModel != null)
            {
                _context.Workers.Remove(workerModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerModelExists(int id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
