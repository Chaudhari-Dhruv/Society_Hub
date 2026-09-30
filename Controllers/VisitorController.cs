using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class VisitorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visitor
        public async Task<IActionResult> Index()
        {
            var visitors = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .ToListAsync();

            return View(visitors);
        }

        // GET: Visitor/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visitor == null)
                return NotFound();

            return View(visitor);
        }

        // GET: Visitor/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View();
        }

        // POST: Visitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Visitor visitor)
        {
            if (ModelState.IsValid)
            {
                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View(visitor);
        }

        // GET: Visitor/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null)
                return NotFound();

            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View(visitor);
        }

        // POST: Visitor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Visitor visitor)
        {
            if (id != visitor.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Visitors.Update(visitor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View(visitor);
        }

        // GET: Visitor/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visitor == null)
                return NotFound();

            return View(visitor);
        }

        // POST: Visitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor != null)
            {
                _context.Visitors.Remove(visitor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        // POST: Visitor/CheckIn/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null)
                return NotFound();

            // Only expected visitors can check in
            if (visitor.Status == "Expected")
            {
                visitor.Status = "Checked-In";
                visitor.EntryTime = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: Visitor/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null)
                return NotFound();

            // Only checked-in visitors can check out
            if (visitor.Status == "Checked-In")
            {
                visitor.Status = "Checked-Out";
                visitor.ExitTime = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}