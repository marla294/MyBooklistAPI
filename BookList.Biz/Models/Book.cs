namespace BookList.Biz.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }

        public Book() {
            Id = 0;
            Title = "";
            URL = "";
            Author = "";
            Cover = "";
        }

        public Book(int id, string name, string url, string author, string cover) {
            Id = id;
            Title = name;
            URL = url;
            Author = author;
            Cover = cover;
        }

        public BookDTO ToDTO() {
            return new BookDTO(Id, Title, URL);
        }
    }
}
