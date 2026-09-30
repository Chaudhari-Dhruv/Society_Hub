using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComplaintController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Complaint
        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints
                .Include(c => c.Resident)
                .ThenInclude(r => r!.Flat)
                .ToListAsync();

            return View(complaints);
        }

        // GET: Complaint/Details/5
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Residents = await _context.Residents
                .Include(r => r.Flat)
                .ToListAsync();

            return View();
        }
        // POST: Complaint/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(int id, Complaint complaint)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);

            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartWork(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);

            if (complaint == null)
                return NotFound();

            complaint.Status = "In Progress";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolve(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);

            if (complaint == null)
                return NotFound();

            complaint.Status = "Resolved";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Complaint/MyComplaints
        public async Task<IActionResult> MyComplaints(int residentId)
        {
            var complaints = await _context.Complaints
                .Include(c => c.Resident)
                .ThenInclude(r => r!.Flat)
                .Where(c => c.ResidentId == residentId)
                .ToListAsync();

            return View(complaints);
        }
    }
}