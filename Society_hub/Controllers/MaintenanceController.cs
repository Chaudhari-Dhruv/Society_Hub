using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class MaintenanceBillController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MaintenanceBillController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // ADMIN ACTIONS
        // =========================

        // GET: MaintenanceBill
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var bills = await _context.MaintenanceBills
                .Include(b => b.Resident)
                .OrderByDescending(b => b.BillingMonth)
                .ToListAsync();

            return View(bills);
        }

        // GET: MaintenanceBill/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var bill = await _context.MaintenanceBills
                .Include(b => b.Resident)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        // GET: MaintenanceBill/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Residents = await _context.Residents
                .OrderBy(r => r.FullName)
                .ToListAsync();

            return View();
        }

        // POST: MaintenanceBill/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(MaintenanceBill bill)
        {
            if (ModelState.IsValid)
            {
                bill.CreatedDate = DateTime.Now;
                bill.PaymentStatus = "Pending";

                _context.MaintenanceBills.Add(bill);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Residents = await _context.Residents
                .OrderBy(r => r.FullName)
                .ToListAsync();

            return View(bill);
        }

        // GET: MaintenanceBill/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var bill = await _context.MaintenanceBills
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            ViewBag.Residents = await _context.Residents
                .OrderBy(r => r.FullName)
                .ToListAsync();

            return View(bill);
        }

        // POST: MaintenanceBill/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int id,
            MaintenanceBill bill)
        {
            if (id != bill.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.MaintenanceBills.Update(bill);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Residents = await _context.Residents
                .OrderBy(r => r.FullName)
                .ToListAsync();

            return View(bill);
        }

        // GET: MaintenanceBill/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var bill = await _context.MaintenanceBills
                .Include(b => b.Resident)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        // POST: MaintenanceBill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.MaintenanceBills
                .FindAsync(id);

            if (bill != null)
            {
                _context.MaintenanceBills.Remove(bill);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: MaintenanceBill/MarkAsPaid/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkAsPaid(
            int id,
            DateTime paymentDate,
            string receiptNumber)
        {
            var bill = await _context.MaintenanceBills
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            bill.PaymentStatus = "Paid";
            bill.PaymentDate = paymentDate;
            bill.ReceiptNumber = receiptNumber;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // RESIDENT ACTIONS
        // =========================

        // GET: MaintenanceBill/MyBills
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> MyBills()
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

            var bills = await _context.MaintenanceBills
                .Include(b => b.Resident)
                .Where(b => b.ResidentId == resident.Id)
                .OrderByDescending(b => b.BillingMonth)
                .ToListAsync();

            return View(bills);
        }

        // GET: MaintenanceBill/PaymentHistory
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> PaymentHistory()
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

            var payments = await _context.MaintenanceBills
                .Include(b => b.Resident)
                .Where(b =>
                    b.ResidentId == resident.Id &&
                    b.PaymentStatus == "Paid")
                .OrderByDescending(b => b.PaymentDate)
                .ToListAsync();

            return View(payments);
        }

        // GET: MaintenanceBill/Receipt/5
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> Receipt(int id)
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

            var bill = await _context.MaintenanceBills
                .Include(b => b.Resident)
                .FirstOrDefaultAsync(b =>
                    b.Id == id &&
                    b.ResidentId == resident.Id &&
                    b.PaymentStatus == "Paid");

            if (bill == null)
                return NotFound();

            return View(bill);
        }
    }
}