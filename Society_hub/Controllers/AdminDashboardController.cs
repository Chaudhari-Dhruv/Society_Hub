using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;
using Society_hub.Models.ViewModels;

namespace Society_hub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;

            var model = new AdminDashboardViewModel
            {
                TotalResidents = await _context.Residents.CountAsync(),

                TotalFlats = await _context.Flats.CountAsync(),

                OccupiedFlats = await _context.Flats
                    .CountAsync(f => f.IsOccupied),

                VacantFlats = await _context.Flats
                    .CountAsync(f => !f.IsOccupied),

                PendingComplaints = await _context.Complaints
                    .CountAsync(c => c.Status == "Pending"),

                TodaysVisitors = await _context.Visitors
                    .CountAsync(v => v.VisitDate.Date == today),

                UpcomingEvents = await _context.Events
                    .CountAsync(e => e.EventDate >= DateTime.Now)
            };

            return View(model);
        }
    }
}