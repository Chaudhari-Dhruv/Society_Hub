using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApartmentBlockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApartmentBlockController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(
                await _context.ApartmentBlocks.ToListAsync()
            );
        }

        public async Task<IActionResult> Details(int id)
        {
            var block = await _context.ApartmentBlocks
                .Include(b => b.Flats)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (block == null)
                return NotFound();

            return View(block);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ApartmentBlock block)
        {
            if (ModelState.IsValid)
            {
                _context.ApartmentBlocks.Add(block);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(block);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var block = await _context.ApartmentBlocks
                .FindAsync(id);

            if (block == null)
                return NotFound();

            return View(block);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ApartmentBlock block)
        {
            if (id != block.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.ApartmentBlocks.Update(block);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(block);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var block = await _context.ApartmentBlocks
                .FindAsync(id);

            if (block == null)
                return NotFound();

            return View(block);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var block = await _context.ApartmentBlocks
                .FindAsync(id);

            if (block != null)
            {
                _context.ApartmentBlocks.Remove(block);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}