namespace BookList.Biz.Models
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

        public BookDTO()
        {
            Id = 0;
            Name = "";
            URL = "";
        }

        public BookDTO(int id, string name, string url)
        {
            Id = id;
            Name = name;
            URL = url;
        }
    }
}
