namespace Library_lab_3.ViewModels
{
    public class BookListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public List<string> Authors { get; set; }
            = new List<string>();
    }
}