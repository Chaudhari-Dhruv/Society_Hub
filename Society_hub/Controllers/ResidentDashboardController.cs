using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;
using Society_hub.Models.ViewModels;

namespace Society_hub.Controllers
{
    [Authorize(Roles = "Resident")]
    public class ResidentDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResidentDashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get currently logged-in Identity user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            // Find Resident connected to this Identity user
            var resident = await _context.Residents
                .Include(r => r.Flat)
                .ThenInclude(f => f!.ApartmentBlock)
                .FirstOrDefaultAsync(
                    r => r.ApplicationUserId == user.Id);

            if (resident == null)
            {
                return NotFound(
                    "Resident profile not found for this account.");
            }

            int residentId = resident.Id;

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
                        v.VisitDate >= DateTime.Now),

                NoticeCount = await _context.Notices
                    .CountAsync(),

                UpcomingEventCount = await _context.Events
                    .CountAsync(e =>
                        e.EventDate >= DateTime.Now)
            };

            return View(model);
        }
    }
}