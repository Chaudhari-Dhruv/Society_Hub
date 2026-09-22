using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Events
                .Include(e => e.Registrations)
                .OrderBy(e => e.EventDate)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(eventModel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(eventModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var eventModel = await _context.Events
                .Include(e => e.Registrations)
                .ThenInclude(r => r.Resident)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(int eventId, int residentId)
        {
            var registration = new EventRegistration
            {
                EventId = eventId,
                ResidentId = residentId,
                RegistrationDate = DateTime.Now
            };

            _context.EventRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = eventId });
        }
    }
}