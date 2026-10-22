using Library_lab_3.Models;
using Library_lab_3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library_lab_3.Controllers
{
    public class LoansController : Controller
    {
        private readonly LibraryDbContext _context;

        public LoansController(LibraryDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var loans = await _context.Loans
                .AsNoTracking()
                .Select(l => new LoanListViewModel
                {
                    Id = l.Id,

                    Reader =
                        l.Reader.LastName + " " +
                        l.Reader.FirstName,

                    Book = l.Book.Title,

                    Authors = l.Book.Authors
                        .Select(a =>
                            a.LastName + " " +
                            a.FirstName + " " +
                            (a.MiddleName ?? "")
                        )
                        .ToList(),

                    IssueDate = l.IssueDate,

                    ReturnDate = l.ReturnDate
                })

                .OrderByDescending(l => l.IssueDate)

                .ToListAsync();

            return View(loans);
        }


        public async Task<IActionResult> Create()
        {
            var loan = new Loan
            {
                IssueDate = DateTime.Today,
                ReturnDate = DateTime.Today
            };

            await LoadSelectLists();

            return View(loan);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            if (loan.ReturnDate < loan.IssueDate)
            {
                ModelState.AddModelError(
                    nameof(loan.ReturnDate),
                    "Дата повернення не може бути раніше дати видачі."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectLists(
                    loan.ReaderId,
                    loan.BookId
                );

                return View(loan);
            }

            _context.Loans.Add(loan);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var loan =
                await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            await LoadSelectLists(
                loan.ReaderId,
                loan.BookId
            );

            return View(loan);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (loan.ReturnDate < loan.IssueDate)
            {
                ModelState.AddModelError(
                    nameof(loan.ReturnDate),
                    "Дата повернення не може бути раніше дати видачі."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectLists(
                    loan.ReaderId,
                    loan.BookId
                );

                return View(loan);
            }

            
            var existingLoan =
                await _context.Loans.FindAsync(id);

            if (existingLoan == null)
            {
                return NotFound();
            }

            existingLoan.ReaderId = loan.ReaderId;
            existingLoan.BookId = loan.BookId;
            existingLoan.IssueDate = loan.IssueDate;
            existingLoan.ReturnDate = loan.ReturnDate;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            
            var loan = await _context.Loans
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new Loan
                {
                    Id = l.Id,

                    IssueDate = l.IssueDate,

                    ReturnDate = l.ReturnDate,

                    Reader = new Reader
                    {
                        LastName = l.Reader.LastName,
                        FirstName = l.Reader.FirstName
                    },

                    Book = new Book
                    {
                        Title = l.Book.Title
                    }
                })
                .FirstOrDefaultAsync();

            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan =
                await _context.Loans.FindAsync(id);

            if (loan != null)
            {
                _context.Loans.Remove(loan);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task LoadSelectLists(
            int? readerId = null,
            int? bookId = null)
        {
           
            var readers = await _context.Readers
                .AsNoTracking()
                .OrderBy(r => r.LastName)
                .Select(r => new
                {
                    r.Id,

                    Name =
                        r.LastName + " " +
                        r.FirstName
                })
                .ToListAsync();

            var books = await _context.Books
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .Select(b => new
                {
                    b.Id,
                    b.Title
                })
                .ToListAsync();


            ViewBag.ReaderId = new SelectList(
                readers,
                "Id",
                "Name",
                readerId
            );

            ViewBag.BookId = new SelectList(
                books,
                "Id",
                "Title",
                bookId
            );
        }
    }
}