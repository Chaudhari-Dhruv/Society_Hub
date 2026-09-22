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

        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints
                .Include(c => c.Resident)
                .ToListAsync();

            return View(complaints);
        }

        public async Task<IActionResult> Details(int id)
        {
            var complaint = await _context.Complaints
                .Include(c => c.Resident)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
                return NotFound();

            return View(complaint);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                complaint.Status = "Pending";
                complaint.CreatedAt = DateTime.Now;

                _context.Complaints.Add(complaint);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(complaint);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var complaint = await _context.Complaints.FindAsync(id);

            if (complaint == null)
                return NotFound();

            complaint.Status = status;

            if (status == "Resolved")
                complaint.ResolvedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}