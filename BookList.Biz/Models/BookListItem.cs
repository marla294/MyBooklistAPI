namespace BookList.Biz.Models
{
    public class BookListItem
    {
        int Id { get; set; }
        BookDTO Book { get; set; }
        UserDTO User { get; set; }
        bool Done { get; set; }
        int Rating { get; set; }
        string Notes { get; set; }
        int SortOrder { get; set; }

        public BookListItem() {
            Id = 0;
            Book = null;
            User = null;
            Done = false;
            Rating = 0;
            Notes = "";
            SortOrder = 0;
        }

        public BookListItem(int id, BookDTO book, UserDTO user, bool done, int rating, 
                        string notes, int sortOrder) {
            Id = id;
            Book = book;
            User = user;
            Done = done;
            Rating = rating;
            Notes = notes;
            SortOrder = sortOrder;
        }

        public BookListItemDTO ToDTO()
        {
            return new BookListItemDTO(Id, Book, User, Done, Rating, Notes, 
                                       SortOrder);
        }
    }
}
