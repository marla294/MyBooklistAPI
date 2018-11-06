namespace BookList.Biz.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

        public Book() {
            Id = 0;
            Name = "";
            URL = "";
        }

        public Book(int id, string name, string url) {
            Id = id;
            Name = name;
            URL = url;
        }

        public BookDTO ToDTO() {
            return new BookDTO(Id, Name, URL);
        }
    }
}
