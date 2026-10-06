using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_hub.Data;
using Society_hub.Models;

namespace Society_hub.Controllers
{
    public class NoticeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoticeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notice
        // Admin + Resident can view notices
        [Authorize(Roles = "Admin,Resident")]
        public async Task<IActionResult> Index()
        {
            var notices = await _context.Notices
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return View(notices);
        }

        // GET: Notice/Details/5
        // Admin + Resident can view notice details
        [Authorize(Roles = "Admin,Resident")]
        public async Task<IActionResult> Details(int id)
        {
            var notice = await _context.Notices
                .FirstOrDefaultAsync(n => n.Id == id);

            if (notice == null)
                return NotFound();

            return View(notice);
        }

        // GET: Notice/Create
        // Admin only
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notice/Create
        // Admin only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.CreatedDate = DateTime.Now;
                notice.PublishedDate = DateTime.Now;

                _context.Notices.Add(notice);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(notice);
        }

        // GET: Notice/Edit/5
        // Admin only
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var notice = await _context.Notices
                .FindAsync(id);

            if (notice == null)
                return NotFound();

            return View(notice);
        }

        // POST: Notice/Edit/5
        // Admin only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int id,
            Notice notice)
        {
            if (id != notice.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Notices.Update(notice);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(notice);
        }

        // GET: Notice/Delete/5
        // Admin only
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var notice = await _context.Notices
                .FirstOrDefaultAsync(n => n.Id == id);

            if (notice == null)
                return NotFound();

            return View(notice);
        }

        // POST: Notice/Delete/5
        // Admin only
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notice = await _context.Notices
                .FindAsync(id);

            if (notice != null)
            {
                _context.Notices.Remove(notice);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Notice/ResidentNotices
        // Resident only
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> ResidentNotices()
        {
            var notices = await _context.Notices
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return View(notices);
        }
    }
}