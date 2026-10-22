using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Library_lab_3.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public int ReaderId { get; set; }

        public int BookId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ReturnDate { get; set; }
        [ValidateNever]
        public Reader Reader { get; set; } = null!;
        [ValidateNever]
        public Book Book { get; set; } = null!;
    }
}