namespace Library_lab_3.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public ICollection<Author> Authors { get; set; }
            = new List<Author>();

        public ICollection<Loan> Loans { get; set; }
            = new List<Loan>();
    }
}