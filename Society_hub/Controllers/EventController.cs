using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // ADMIN + RESIDENT
        // =========================

        // GET: Event
        [Authorize(Roles = "Admin,Resident")]
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        [Authorize(Roles = "Admin,Resident")]
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            // Admin can see all registrations
            if (User.IsInRole("Admin"))
            {
                eventItem.EventRegistrations = await _context.EventRegistrations
                    .Include(r => r.Resident)
                    .Where(r => r.EventId == id)
                    .ToListAsync();

                return View(eventItem);
            }

            // Resident can see only their own registration
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var resident = await _context.Residents
                .FirstOrDefaultAsync(r => r.ApplicationUserId == user.Id);

            if (resident == null)
                return NotFound("Resident profile not found for this account.");

            eventItem.EventRegistrations = await _context.EventRegistrations
                .Where(r =>
                    r.EventId == id &&
                    r.ResidentId == resident.Id)
                .ToListAsync();

            return View(eventItem);
        }

        // =========================
        // ADMIN ONLY
        // =========================

        // GET: Event/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Event eventItem)
        {
            if (ModelState.IsValid)
            {
                eventItem.CreatedDate = DateTime.Now;

                _context.Events.Add(eventItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(eventItem);
        }

        // GET: Event/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events
                .FindAsync(id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int id,
            Event eventItem)
        {
            if (id != eventItem.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Events.Update(eventItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(eventItem);
        }

        // GET: Event/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events
                .FindAsync(id);

            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Event/Registrations/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Registrations(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.EventRegistrations)
                .ThenInclude(r => r.Resident)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // =========================
        // RESIDENT ONLY
        // =========================

        // GET: Event/Register/5
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> Register(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // POST: Event/Register/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> RegisterConfirmed(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var resident = await _context.Residents
                .FirstOrDefaultAsync(r =>
                    r.ApplicationUserId == user.Id);

            if (resident == null)
            {
                return NotFound(
                    "Resident profile not found for this account.");
            }

            var alreadyRegistered =
                await _context.EventRegistrations
                    .AnyAsync(r =>
                        r.EventId == id &&
                        r.ResidentId == resident.Id);

            if (!alreadyRegistered)
            {
                var registration = new EventRegistration
                {
                    EventId = id,
                    ResidentId = resident.Id,
                    RegistrationDate = DateTime.Now
                };

                _context.EventRegistrations.Add(registration);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(
                nameof(Details),
                new { id = id });
        }

        // POST: Event/CancelRegistration/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> CancelRegistration(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var resident = await _context.Residents
                .FirstOrDefaultAsync(r =>
                    r.ApplicationUserId == user.Id);

            if (resident == null)
            {
                return NotFound(
                    "Resident profile not found for this account.");
            }

            var registration =
                await _context.EventRegistrations
                    .FirstOrDefaultAsync(r =>
                        r.EventId == id &&
                        r.ResidentId == resident.Id);

            if (registration != null)
            {
                _context.EventRegistrations.Remove(registration);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(
                nameof(Details),
                new { id = id });
        }

        // GET: Event/ResidentEvents
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> ResidentEvents()
        {
            var events = await _context.Events
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }
    }
}