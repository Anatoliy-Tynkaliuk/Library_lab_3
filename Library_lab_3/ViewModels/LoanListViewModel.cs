namespace Library_lab_3.ViewModels
{
    public class LoanListViewModel
    {
        public int Id { get; set; }

        public string Reader { get; set; } = null!;

        public string Book { get; set; } = null!;

        public List<string> Authors { get; set; }
            = new List<string>();

        public DateTime IssueDate { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}