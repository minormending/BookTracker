namespace BookTracker.Services
{
    public class BookService
    {
        private readonly List<Book> _books = new();

        public async Task<List<Book>> GetBooksAsync()
        {
            await Task.Delay(500);
            return _books;
        }

        public async Task AddBook(string title, string? author)
        {
            _books.Add(new Book()
            {
                Id = _books.Count + 1,
                Title = title,
                Author = author,
                IsCompleted = false,
            });
            await Task.Delay(500);
        }

        public (int TotalBooks, int CompletedBooks) CalculateStats(IEnumerable<Book> books)
        {
            int CountCompletedBooks() => books.Count(b => b.IsCompleted);
            return (books.Count(), CountCompletedBooks());
        }

        public string GetBookCategory(string genre) =>
            genre switch
            {
                "fiction" => "Entertainment",
                "non-fiction" => "Education",
                "mystery" => "Thriller",
                _ => "General"
            };
    }
}
