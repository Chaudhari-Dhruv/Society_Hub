using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SecurityGuardManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SecurityGuardManagementController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // LIST SECURITY GUARDS
        // =========================

        // GET: SecurityGuardManagement
        public async Task<IActionResult> Index()
        {
            var guards = await _context.SecurityGuards
                .Include(g => g.ApplicationUser)
                .ToListAsync();

            return View(guards);
        }


        // =========================
        // CREATE SECURITY GUARD
        // =========================

        // GET: SecurityGuardManagement/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: SecurityGuardManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string fullName,
            string email,
            string password,
            string phone,
            string shift)
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(shift))
            {
                ModelState.AddModelError(
                    "",
                    "All fields are required.");

                return View();
            }

            // Create Identity account
            var user = new ApplicationUser
            {
                FullName = fullName,
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(
                user,
                password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                return View();
            }

            // Assign SecurityGuard role
            await _userManager.AddToRoleAsync(
                user,
                "SecurityGuard");

            // Create SecurityGuard profile
            var guard = new SecurityGuard
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Shift = shift,
                IsActive = true,

                // Connect profile with Identity account
                ApplicationUserId = user.Id
            };

            _context.SecurityGuards.Add(guard);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // DETAILS
        // =========================

        // GET: SecurityGuardManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var guard = await _context.SecurityGuards
                .Include(g => g.ApplicationUser)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guard == null)
                return NotFound();

            return View(guard);
        }


        // =========================
        // DELETE
        // =========================

        // GET: SecurityGuardManagement/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var guard = await _context.SecurityGuards
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guard == null)
                return NotFound();

            return View(guard);
        }


        // POST: SecurityGuardManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guard = await _context.SecurityGuards
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guard == null)
                return NotFound();

            // Delete Identity account also
            if (!string.IsNullOrEmpty(guard.ApplicationUserId))
            {
                var user = await _userManager.FindByIdAsync(
                    guard.ApplicationUserId);

                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            _context.SecurityGuards.Remove(guard);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}