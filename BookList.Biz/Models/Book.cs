namespace BookList.Biz.Models
{
    public class Book
    {
        int Id { get; set; }
        string Name { get; set; }
        string URL { get; set; }

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
