using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;
using Society_hub.Models.ViewModels;

namespace Society_hub.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        // =========================
        // REGISTER
        // =========================

        // GET: Account/Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToUserDashboard();
            }

            await PopulateFlatsViewBagAsync();
            var model = new RegisterViewModel();
            return View(model);
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateFlatsViewBagAsync();
                return View(model);
            }

            // Normalize and validate role
            var role = model.Role?.Trim();
            if (role != "Admin" && role != "Resident" && role != "SecurityGuard")
            {
                ModelState.AddModelError("Role", "Please select a valid role (Admin, Resident, or SecurityGuard).");
                await PopulateFlatsViewBagAsync();
                return View(model);
            }

            // Ensure role exists in the database
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            // Create ApplicationUser
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign Selected Role
                await _userManager.AddToRoleAsync(user, role);

                // Handle Role-Specific Records
                if (role == "Resident")
                {
                    var resident = new Resident
                    {
                        FullName = model.FullName,
                        Email = model.Email,
                        Phone = model.Phone ?? string.Empty,
                        FamilyMembers = model.FamilyMembers > 0 ? model.FamilyMembers : 1,
                        FlatId = model.FlatId,
                        ApplicationUserId = user.Id
                    };

                    _context.Residents.Add(resident);

                    if (model.FlatId.HasValue)
                    {
                        var flat = await _context.Flats.FindAsync(model.FlatId.Value);
                        if (flat != null)
                        {
                            flat.IsOccupied = true;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                else if (role == "SecurityGuard")
                {
                    var guard = new SecurityGuard
                    {
                        FullName = model.FullName,
                        Email = model.Email,
                        Phone = !string.IsNullOrWhiteSpace(model.GuardPhone) ? model.GuardPhone : (model.Phone ?? string.Empty),
                        Shift = !string.IsNullOrWhiteSpace(model.Shift) ? model.Shift : "Morning",
                        IsActive = true,
                        ApplicationUserId = user.Id
                    };

                    _context.SecurityGuards.Add(guard);
                    await _context.SaveChangesAsync();
                }
                // If "Admin", only ApplicationUser with "Admin" role is needed.

                // Automatically sign in the newly registered user
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect to the appropriate dashboard
                return RedirectForRole(role);
            }

            // Add Identity errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            await PopulateFlatsViewBagAsync();
            return View(model);
        }

        // =========================
        // LOGIN
        // =========================

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToUserDashboard();
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "AdminDashboard");
                    }
                    if (await _userManager.IsInRoleAsync(user, "SecurityGuard"))
                    {
                        return RedirectToAction("Dashboard", "SecurityGuard");
                    }
                    if (await _userManager.IsInRoleAsync(user, "Resident"))
                    {
                        return RedirectToAction("Index", "ResidentDashboard");
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View();
        }

        // =========================
        // LOGOUT
        // =========================

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // =========================
        // ACCESS DENIED
        // =========================

        // GET: Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // =========================
        // HELPERS
        // =========================

        private async Task PopulateFlatsViewBagAsync()
        {
            ViewBag.Flats = await _context.Flats
                .Include(f => f.ApartmentBlock)
                .OrderBy(f => f.ApartmentBlock != null ? f.ApartmentBlock.BlockName : "")
                .ThenBy(f => f.FlatNumber)
                .ToListAsync();
        }

        private IActionResult RedirectToUserDashboard()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
            if (User.IsInRole("SecurityGuard"))
            {
                return RedirectToAction("Dashboard", "SecurityGuard");
            }
            if (User.IsInRole("Resident"))
            {
                return RedirectToAction("Index", "ResidentDashboard");
            }
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectForRole(string role)
        {
            return role switch
            {
                "Admin" => RedirectToAction("Index", "AdminDashboard"),
                "SecurityGuard" => RedirectToAction("Dashboard", "SecurityGuard"),
                "Resident" => RedirectToAction("Index", "ResidentDashboard"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}