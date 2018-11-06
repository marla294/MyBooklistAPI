using System;
using System.Collections.Generic;
using BookList.Biz.Models;

namespace BookList.Biz.Database
{
    public static class LoadItems
    {
        public static List<BookListItem> LoadAll()
        {
            return LoadByQuery("select * from booklist order by id;");
        }

        static List<BookListItem> LoadByQuery(string sql)
        {
            var itemResultSet = ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
            var itemList = new List<BookListItem>();

            for (var i = 0; i < itemResultSet[0].Count; i++)
            {
                BookListItem item = Int32.TryParse(itemResultSet[0][i], out int id) ? LoadOneItem(itemResultSet, i) : new BookListItem();

                itemList.Add(item);
            }

            return itemList;
        }

        static BookListItem LoadOneItem(List<List<string>> itemResultSet, int row) {
            int id = Int32.TryParse(itemResultSet[0][row], out int _id) ? _id : 0;
            Book book = Int32.TryParse(itemResultSet[1][row], out int bookId) ? LoadBook.LoadSingle(bookId) : new Book();
            User user = Int32.TryParse(itemResultSet[2][row], out int userId) ? LoadUsers.LoadSingle(userId) : new User();
            bool done = bool.TryParse(itemResultSet[3][row], out bool d) && d;
            int rating = Int32.TryParse(itemResultSet[4][row], out int r) ? r : 0;
            string notes = itemResultSet[5][row];
            int sortOrder = Int32.TryParse(itemResultSet[6][row], out int s) ? s : 0;

            return new BookListItem(id, book, user, done, rating, notes, sortOrder);
        }
    }
}
