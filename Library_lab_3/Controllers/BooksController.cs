using Library_lab_3.Models;
using Library_lab_3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_lab_3.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .AsNoTracking()

                .Select(b => new BookListViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Authors = b.Authors
                        .Select(a =>
                            a.LastName + " " +
                            a.FirstName + " " +
                            (a.MiddleName ?? "")
                        )
                        .ToList()
                })

                .OrderBy(b => b.Title)

                .ToListAsync();

            return View(books);
        }


        public async Task<IActionResult> Create()
        {
            
            ViewBag.Authors = await _context.Authors
                .AsNoTracking()
                .OrderBy(a => a.LastName)
                .ToListAsync();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Book book,
            int[] authorIds)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = await _context.Authors
                    .AsNoTracking()
                    .OrderBy(a => a.LastName)
                    .ToListAsync();

                return View(book);
            }

            if (authorIds.Length > 0)
            {
                var authors = await _context.Authors
                    .Where(a => authorIds.Contains(a.Id))
                    .ToListAsync();

                foreach (var author in authors)
                {
                    book.Authors.Add(author);
                }
            }

            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            
            var book = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            ViewBag.Authors = await _context.Authors
                .AsNoTracking()
                .OrderBy(a => a.LastName)
                .ToListAsync();

            ViewBag.SelectedAuthorIds =
                book.Authors
                    .Select(a => a.Id)
                    .ToArray();

            return View(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Book book,
            int[] authorIds)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            var existingBook = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBook == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Authors = await _context.Authors
                    .AsNoTracking()
                    .OrderBy(a => a.LastName)
                    .ToListAsync();

                ViewBag.SelectedAuthorIds = authorIds;

                return View(book);
            }

            existingBook.Title = book.Title;
            existingBook.Price = book.Price;

            existingBook.Authors.Clear();

            if (authorIds.Length > 0)
            {
                var authors = await _context.Authors
                    .Where(a => authorIds.Contains(a.Id))
                    .ToListAsync();

                foreach (var author in authors)
                {
                    existingBook.Authors.Add(author);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            
            var book = await _context.Books
                .AsNoTracking()
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book != null)
            {
                _context.Books.Remove(book);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}