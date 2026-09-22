using Microsoft.AspNetCore.Mvc;
using Society_hub.Data;
using System.Linq;

namespace Society_hub.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalResidents = _context.Residents.Count();

            ViewBag.TotalFlats = _context.Flats.Count();

            ViewBag.OccupiedFlats =
                _context.Flats.Count(f => f.IsOccupied);

            ViewBag.VacantFlats =
                _context.Flats.Count(f => !f.IsOccupied);

            ViewBag.PendingComplaints =
                _context.Complaints.Count(c => c.Status == "Pending");

            ViewBag.TodayVisitors =
                _context.Visitors.Count(v =>
                    v.VisitDate.Date == DateTime.Today);

            ViewBag.UpcomingEvents =
                _context.Events.Count(e =>
                    e.EventDate >= DateTime.Now);

            return View();
        }
    }
}