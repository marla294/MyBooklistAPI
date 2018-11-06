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
                BookListItem item;

                if (Int32.TryParse(itemResultSet[0][i], out int id)) {
                    Book book = Int32.TryParse(itemResultSet[1][i], out int bookId) ? LoadBook.LoadSingle(bookId) : new Book();
                    User user = Int32.TryParse(itemResultSet[2][i], out int userId) ? LoadUsers.LoadSingle(userId) : new User();
                    bool done = bool.TryParse(itemResultSet[3][i], out bool d) && d;
                    int rating = Int32.TryParse(itemResultSet[4][i], out int r) ? r : 0;
                    string notes = itemResultSet[5][i];
                    int sortOrder = Int32.TryParse(itemResultSet[6][i], out int s) ? s : 0;

                    item = new BookListItem(id, book, user, done, rating, notes, sortOrder);
                } else {
                    item = new BookListItem();
                }

                itemList.Add(item);
            }

            return itemList;
        }
    }
}
