using Library_lab_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_lab_3.Controllers
{
    public class ReadersController : Controller
    {
        private readonly LibraryDbContext _context;

        public ReadersController(
            LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var readers = await _context.Readers
                .AsNoTracking()
                .ToListAsync();

            return View(readers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Reader reader)
        {
            if (!ModelState.IsValid)
                return View(reader);

            _context.Readers.Add(reader);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reader =
                await _context.Readers.FindAsync(id);

            if (reader == null)
                return NotFound();

            return View(reader);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            Reader reader)
        {
            if (id != reader.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(reader);

            _context.Readers.Update(reader);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reader =
                await _context.Readers.FindAsync(id);

            if (reader == null)
                return NotFound();

            return View(reader);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var reader =
                await _context.Readers.FindAsync(id);

            if (reader != null)
            {
                _context.Readers.Remove(reader);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}