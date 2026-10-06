using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class VisitorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VisitorController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // ADMIN + SECURITY GUARD
        // =========================

        // GET: Visitor
        [Authorize(Roles = "Admin,SecurityGuard")]
        public async Task<IActionResult> Index()
        {
            var visitors = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .ToListAsync();

            return View(visitors);
        }

        // GET: Visitor/Details/5
        [Authorize(Roles = "Admin,SecurityGuard")]
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


        // =========================
        // ADMIN ONLY
        // =========================

        // GET: Visitor/Create
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Visitor visitor)
        {
            if (ModelState.IsValid)
            {
                visitor.Status = "Expected";
                visitor.EntryTime = null;
                visitor.ExitTime = null;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var visitor = await _context.Visitors
                .FirstOrDefaultAsync(v => v.Id == id);

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int id,
            Visitor visitor)
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitor = await _context.Visitors
                .FindAsync(id);

            if (visitor != null)
            {
                _context.Visitors.Remove(visitor);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // SECURITY GUARD ONLY
        // =========================

        // POST: Visitor/CheckIn/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SecurityGuard")]
        public async Task<IActionResult> CheckIn(int id, string? returnUrl = null)
        {
            var visitor = await _context.Visitors
                .FindAsync(id);

            if (visitor == null)
                return NotFound();

            if (visitor.Status == "Expected")
            {
                visitor.Status = "Checked-In";
                visitor.EntryTime = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Visitor/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SecurityGuard")]
        public async Task<IActionResult> CheckOut(int id, string? returnUrl = null)
        {
            var visitor = await _context.Visitors
                .FindAsync(id);

            if (visitor == null)
                return NotFound();

            if (visitor.Status == "Checked-In")
            {
                visitor.Status = "Checked-Out";
                visitor.ExitTime = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // RESIDENT ONLY
        // =========================

        // GET: Visitor/CreateForResident
        [Authorize(Roles = "Resident")]
        public IActionResult CreateForResident()
        {
            return View();
        }

        // POST: Visitor/CreateForResident
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> CreateForResident(
            Visitor visitor)
        {
            // Get logged-in Identity user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            // Find connected Resident
            var resident = await _context.Residents
                .FirstOrDefaultAsync(r =>
                    r.ApplicationUserId == user.Id);

            if (resident == null)
            {
                return NotFound(
                    "Resident profile not found for this account.");
            }

            if (ModelState.IsValid)
            {
                // Automatically connect visitor
                // to the logged-in resident.
                visitor.ResidentId = resident.Id;

                // New visitor always starts as Expected.
                visitor.Status = "Expected";

                // These are set only when Security Guard
                // actually checks the visitor in/out.
                visitor.EntryTime = null;
                visitor.ExitTime = null;

                _context.Visitors.Add(visitor);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyVisitors));
            }

            return View(visitor);
        }

        // GET: Visitor/MyVisitors
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> MyVisitors()
        {
            // Get logged-in Identity user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            // Find connected Resident
            var resident = await _context.Residents
                .FirstOrDefaultAsync(r =>
                    r.ApplicationUserId == user.Id);

            if (resident == null)
            {
                return NotFound(
                    "Resident profile not found for this account.");
            }

            // Get only this resident's visitors
            var visitors = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .Where(v => v.ResidentId == resident.Id)
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();

            return View(visitors);
        }
    }
}