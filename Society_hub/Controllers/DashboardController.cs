using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models.ViewModels;

namespace Society_hub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var model = new AdminDashboardViewModel
            {
                TotalResidents = await _context.Residents.CountAsync(),

                TotalFlats = await _context.Flats.CountAsync(),

                OccupiedFlats = await _context.Residents
                    .Select(r => r.FlatId)
                    .Distinct()
                    .CountAsync(),

                VacantFlats = await _context.Flats.CountAsync()
                    - await _context.Residents
                        .Select(r => r.FlatId)
                        .Distinct()
                        .CountAsync(),

                PendingComplaints = await _context.Complaints
                    .CountAsync(c => c.Status == "Pending"),

                TodaysVisitors = await _context.Visitors
                    .CountAsync(v =>
                        v.VisitDate >= today &&
                        v.VisitDate < tomorrow),

                UpcomingEvents = await _context.Events
                    .CountAsync(e => e.EventDate >= today)
            };

            return View(model);
        }
    }
}