using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class SecurityGuardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SecurityGuardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SecurityGuard
        public async Task<IActionResult> Index()
        {
            var guards = await _context.SecurityGuards.ToListAsync();

            return View(guards);
        }

        // GET: SecurityGuard/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var guard = await _context.SecurityGuards
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guard == null)
                return NotFound();

            return View(guard);
        }

        // GET: SecurityGuard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecurityGuard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SecurityGuard guard)
        {
            if (ModelState.IsValid)
            {
                _context.SecurityGuards.Add(guard);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(guard);
        }

        // GET: SecurityGuard/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var guard = await _context.SecurityGuards.FindAsync(id);

            if (guard == null)
                return NotFound();

            return View(guard);
        }

        // POST: SecurityGuard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SecurityGuard guard)
        {
            if (id != guard.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.SecurityGuards.Update(guard);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(guard);
        }

        // GET: SecurityGuard/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var guard = await _context.SecurityGuards
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guard == null)
                return NotFound();

            return View(guard);
        }

        // POST: SecurityGuard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guard = await _context.SecurityGuards.FindAsync(id);

            if (guard != null)
            {
                _context.SecurityGuards.Remove(guard);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: SecurityGuard/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var expectedVisitors = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .Where(v => v.Status == "Expected")
                .ToListAsync();

            var checkedInVisitors = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .Where(v => v.Status == "Checked-In")
                .ToListAsync();

            ViewBag.ExpectedVisitors = expectedVisitors;
            ViewBag.CheckedInVisitors = checkedInVisitors;

            return View();
        }
    }
}