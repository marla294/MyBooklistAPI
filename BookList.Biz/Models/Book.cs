namespace BookList.Biz.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Author { get; set; }

        public Book() {
            Id = 0;
            Title = "";
            URL = "";
            Author = "";
        }

        public Book(int id, string name, string url, string author) {
            Id = id;
            Title = name;
            URL = url;
            Author = author;
        }

        public BookDTO ToDTO() {
            return new BookDTO(Id, Title, URL);
        }
    }
}
