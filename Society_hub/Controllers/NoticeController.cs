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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Notices
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.PublishedDate = DateTime.Now;
                notice.IsActive = true;

                _context.Notices.Add(notice);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(notice);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var notice = await _context.Notices.FindAsync(id);

            if (notice == null)
                return NotFound();

            _context.Notices.Remove(notice);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}