namespace BookList.Biz.Models
{
    public class BookListItemDTO
    {
        public int Id { get; set; }
        public BookDTO Book { get; set; }
        public UserDTO User { get; set; }
        public bool Done { get; set; }
        public int Rating { get; set; }
        public string Notes { get; set; }
        public int SortOrder { get; set; }

        public BookListItemDTO()
        {
            Id = 0;
            Book = null;
            User = null;
            Done = false;
            Rating = 0;
            Notes = "";
            SortOrder = 0;
        }

        public BookListItemDTO(int id, BookDTO book, UserDTO user, bool done, int rating,
                        string notes, int sortOrder)
        {
            Id = id;
            Book = book;
            User = user;
            Done = done;
            Rating = rating;
            Notes = notes;
            SortOrder = sortOrder;
        }
    }
}
