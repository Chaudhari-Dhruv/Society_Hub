using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class FlatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            var flats = await _context.Flats
                .Include(f => f.ApartmentBlock)
                .ToListAsync();

            return View(flats);
        }

        // GET: Flat/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var flat = await _context.Flats.FindAsync(id);

            if (flat == null)
            {
                return NotFound();
            }

            return View(flat);
        }

        // GET: Flat/Create
        public IActionResult Create()
        {
            ViewBag.ApartmentBlocks = _context.ApartmentBlocks.ToList();
            return View();
        }

        // POST: Flat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Flat flat)
        {
            if (ModelState.IsValid)
            {
                _context.Flats.Add(flat);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ApartmentBlocks = _context.ApartmentBlocks.ToList();

            return View(flat);
        }

        // GET: Flat/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var flat = await _context.Flats.FindAsync(id);

            if (flat == null)
            {
                return NotFound();
            }

            return View(flat);
        }

        // POST: Flat/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Flat flat)
        {
            if (id != flat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Flats.Update(flat);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(flat);
        }

        // GET: Flat/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var flat = await _context.Flats.FindAsync(id);

            if (flat == null)
            {
                return NotFound();
            }

            return View(flat);
        }

        // POST: Flat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flat = await _context.Flats.FindAsync(id);

            if (flat != null)
            {
                _context.Flats.Remove(flat);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}