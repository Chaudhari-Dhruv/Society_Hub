using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bills = await _context.MaintenanceBills
                .Include(b => b.Resident)
                .Include(b => b.Payments)
                .ToListAsync();

            return View(bills);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceBill bill)
        {
            if (ModelState.IsValid)
            {
                bill.BillDate = DateTime.Now;
                bill.Status = "Pending";

                _context.MaintenanceBills.Add(bill);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(bill);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var bill = await _context.MaintenanceBills.FindAsync(id);

            if (bill == null)
                return NotFound();

            bill.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}