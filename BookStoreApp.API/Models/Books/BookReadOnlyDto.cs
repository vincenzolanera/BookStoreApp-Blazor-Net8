using BookStoreApp.API.Data;

namespace BookStoreApp.API.Models.Books
{
    public class BookReadOnlyDto : BaseDto
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
