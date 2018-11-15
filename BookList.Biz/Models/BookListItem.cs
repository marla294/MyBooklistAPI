namespace BookList.Biz.Models
{
    public class BookListItem
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public bool Done { get; set; }
        public int Rating { get; set; }
        public string Notes { get; set; }
        public int SortOrder { get; set; }
        public int ListId { get; set; }

        public BookListItem() {
            Id = 0;
            Book = null;
            User = null;
            Done = false;
            Rating = 0;
            Notes = "";
            SortOrder = 0;
            ListId = 0;
        }

        public BookListItem(int id, Book book, User user, bool done, int rating, 
                        string notes, int sortOrder, int listId) {
            Id = id;
            Book = book;
            User = user;
            Done = done;
            Rating = rating;
            Notes = notes;
            SortOrder = sortOrder;
            ListId = listId;
        }

        public BookListItemDTO ToDTO()
        {
            return new BookListItemDTO(Id, Book, User, Done, Rating, Notes, 
                                       SortOrder);
        }
    }
}
