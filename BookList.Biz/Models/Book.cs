namespace BookList.Biz.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Author { get; set; }

        public Book() {
            Id = 0;
            Name = "";
            URL = "";
            Author = "";
        }

        public Book(int id, string name, string url, string author) {
            Id = id;
            Name = name;
            URL = url;
            Author = author;
        }

        public BookDTO ToDTO() {
            return new BookDTO(Id, Name, URL);
        }
    }
}
