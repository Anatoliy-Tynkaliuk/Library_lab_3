namespace Library_lab_3.Models
{
    public class Reader
    {
        public int Id { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string LibraryCardNumber { get; set; } = null!;

        public ICollection<Loan> Loans { get; set; }
            = new List<Loan>();
    }
}


