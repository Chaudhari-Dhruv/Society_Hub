using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComplaintController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // ADMIN ONLY
        // =========================

        // GET: Complaint
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints
                .Include(c => c.Resident)
                .ThenInclude(r => r!.Flat)
                .ToListAsync();

            return View(complaints);
        }

        // GET: Complaint/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var complaint = await _context.Complaints
                .Include(c => c.Resident)
                .ThenInclude(r => r!.Flat)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
                return NotFound();

            return View(complaint);
        }

        // GET: Complaint/Create
        // Admin can create complaint for any resident
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View();
        }

        // POST: Complaint/Create
        // Admin creates complaint for any resident
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                complaint.Status = "Pending";
                complaint.CreatedDate = DateTime.Now;

                _context.Complaints.Add(complaint);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View(complaint);
        }

        // GET: Complaint/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
                return NotFound();

            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View(complaint);
        }

        // POST: Complaint/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int id,
            Complaint complaint)
        {
            if (id != complaint.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Complaints.Update(complaint);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View(complaint);
        }

        // GET: Complaint/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var complaint = await _context.Complaints
                .Include(c => c.Resident)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
                return NotFound();

            return View(complaint);
        }

        // POST: Complaint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var complaint = await _context.Complaints
                .FindAsync(id);

            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Complaint/StartWork/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StartWork(int id)
        {
            var complaint = await _context.Complaints
                .FindAsync(id);

            if (complaint == null)
                return NotFound();

            complaint.Status = "In Progress";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Complaint/Resolve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Resolve(int id)
        {
            var complaint = await _context.Complaints
                .FindAsync(id);

            if (complaint == null)
                return NotFound();

            complaint.Status = "Resolved";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // RESIDENT ONLY
        // =========================

        // GET: Complaint/CreateForResident
        [Authorize(Roles = "Resident")]
        public IActionResult CreateForResident()
        {
            return View();
        }

        // POST: Complaint/CreateForResident
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> CreateForResident(
            Complaint complaint)
        {
            // Get currently logged-in Identity user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            // Find Resident connected to this Identity user
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
                // IMPORTANT:
                // ResidentId comes from logged-in user.
                // Resident cannot select another resident.
                complaint.ResidentId = resident.Id;

                // Automatically set these values
                complaint.Status = "Pending";
                complaint.CreatedDate = DateTime.Now;

                _context.Complaints.Add(complaint);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyComplaints));
            }

            return View(complaint);
        }


        // GET: Complaint/MyComplaints
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> MyComplaints()
        {
            // Get currently logged-in Identity user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            // Find Resident connected to this Identity user
            var resident = await _context.Residents
                .FirstOrDefaultAsync(r =>
                    r.ApplicationUserId == user.Id);

            if (resident == null)
            {
                return NotFound(
                    "Resident profile not found for this account.");
            }

            // Get only this resident's complaints
            var complaints = await _context.Complaints
                .Include(c => c.Resident)
                .ThenInclude(r => r!.Flat)
                .Where(c => c.ResidentId == resident.Id)
                .ToListAsync();

            return View(complaints);
        }
    }
}