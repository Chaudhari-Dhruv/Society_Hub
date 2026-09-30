using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class ResidentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResidentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resident
        public async Task<IActionResult> Index()
        {
            var residents = await _context.Residents
                .Include(r => r.Flat)
                .ThenInclude(f => f!.ApartmentBlock)
                .ToListAsync();

            return View(residents);
        }

        // GET: Resident/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var resident = await _context.Residents
                .Include(r => r.Flat)
                .ThenInclude(f => f!.ApartmentBlock)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resident == null)
                return NotFound();

            return View(resident);
        }

        // GET: Resident/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Flats = await _context.Flats
                .Include(f => f.ApartmentBlock)
                .ToListAsync();

            return View();
        }

        // POST: Resident/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resident resident)
        {
            if (ModelState.IsValid)
            {
                _context.Residents.Add(resident);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Flats = await _context.Flats
                .Include(f => f.ApartmentBlock)
                .ToListAsync();

            return View(resident);
        }

        // GET: Resident/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var resident = await _context.Residents.FindAsync(id);

            if (resident == null)
                return NotFound();

            ViewBag.Flats = await _context.Flats
                .Include(f => f.ApartmentBlock)
                .ToListAsync();

            return View(resident);
        }

        // POST: Resident/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Resident resident)
        {
            if (id != resident.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Residents.Update(resident);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Flats = await _context.Flats
                .Include(f => f.ApartmentBlock)
                .ToListAsync();

            return View(resident);
        }

        // GET: Resident/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var resident = await _context.Residents
                .Include(r => r.Flat)
                .ThenInclude(f => f!.ApartmentBlock)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resident == null)
                return NotFound();

            return View(resident);
        }

        // POST: Resident/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resident = await _context.Residents.FindAsync(id);

            if (resident != null)
            {
                _context.Residents.Remove(resident);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}