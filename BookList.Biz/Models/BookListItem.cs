namespace BookList.Biz.Models
{
    public class BookListItem
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public bool Done { get; set; }
        public int Rating { get; set; }
        public string Notes { get; set; }
        public int SortOrder { get; set; }
        public int ListId { get; set; }

        public BookListItem() {
            Id = 0;
            Book = null;
            Done = false;
            Rating = 0;
            Notes = "";
            SortOrder = 0;
            ListId = 0;
        }

        public BookListItem(int id, Book book, bool done, int rating, 
                        string notes, int sortOrder, int listId) {
            Id = id;
            Book = book;
            Done = done;
            Rating = rating;
            Notes = notes;
            SortOrder = sortOrder;
            ListId = listId;
        }

    }
}
