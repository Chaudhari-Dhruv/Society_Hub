using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    [Authorize(Roles = "SecurityGuard")]
    public class SecurityGuardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SecurityGuardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // SECURITY GUARD DASHBOARD
        // =========================

        // GET: SecurityGuard/Dashboard
        [Authorize(Roles = "SecurityGuard")]
        public async Task<IActionResult> Dashboard()
        {
            var expectedVisitors = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .Where(v => v.Status == "Expected")
                .ToListAsync();

            var checkedInVisitors = await _context.Visitors
                .Include(v => v.Resident)
                .ThenInclude(r => r!.Flat)
                .Where(v => v.Status == "Checked-In")
                .ToListAsync();

            ViewBag.ExpectedVisitors = expectedVisitors;
            ViewBag.CheckedInVisitors = checkedInVisitors;

            // Find logged-in Security Guard
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var guard = await _context.SecurityGuards
                    .FirstOrDefaultAsync(g =>
                        g.ApplicationUserId == user.Id);

                ViewBag.Guard = guard;
            }

            return View();
        }


        // =========================
        // SECURITY GUARD PROFILE
        // =========================

        // GET: SecurityGuard/MyProfile
        [Authorize(Roles = "SecurityGuard")]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var guard = await _context.SecurityGuards
                .FirstOrDefaultAsync(g =>
                    g.ApplicationUserId == user.Id);

            if (guard == null)
            {
                return NotFound(
                    "Security Guard profile not found for this account.");
            }

            return View(guard);
        }
    }
}