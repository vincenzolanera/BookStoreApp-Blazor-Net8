namespace BookStoreApp.API.Models.Books
{
    public class BookDetailDto : BaseDto
    {
        public string Title { get; set; }
        public int Year { get; set; }  
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
