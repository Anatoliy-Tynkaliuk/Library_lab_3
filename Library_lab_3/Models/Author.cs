namespace Library_lab_3.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public ICollection<Book> Books { get; set; }
            = new List<Book>();
    }
}