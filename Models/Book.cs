namespace library_sesterm.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public int Count { get; set; }
        public string Url { get; set; } // Optional: URL for book cover image
    }
}
