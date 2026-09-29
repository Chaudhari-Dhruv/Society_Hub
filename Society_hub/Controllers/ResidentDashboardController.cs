using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models.ViewModels;

namespace Society_hub.Controllers
{
    public class ResidentDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResidentDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Temporary resident ID for testing
            int residentId = 1;

            var resident = await _context.Residents
                .Include(r => r.Flat)
                .ThenInclude(f => f!.ApartmentBlock)
                .FirstOrDefaultAsync(r => r.Id == residentId);

            if (resident == null)
                return NotFound("Resident not found.");

            var today = DateTime.Today;

            var model = new ResidentDashboardViewModel
            {
                Resident = resident,

                ComplaintCount = await _context.Complaints
                    .CountAsync(c => c.ResidentId == residentId),

                PendingComplaintCount = await _context.Complaints
                    .CountAsync(c =>
                        c.ResidentId == residentId &&
                        c.Status == "Pending"),

                UpcomingVisitorCount = await _context.Visitors
                    .CountAsync(v =>
                        v.ResidentId == residentId &&
                        v.VisitDate >= today),

                NoticeCount = await _context.Notices
                    .CountAsync(),

                UpcomingEventCount = await _context.Events
                    .CountAsync(e => e.EventDate >= today)
            };

            return View(model);
        }
    }
}