using System;
using System.Collections.Generic;
using BookList.Biz.Models;

namespace BookList.Biz.Database
{
    public static class ItemFactory
    {
        public static List<BookListItem> LoadAll(IDbConnection dbConnection)
        {
            var itemResultSet = dbConnection.Take("booklist").OrderBy("id").Execute();

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
            Book book = Int32.TryParse(itemResultSet[1][row], out int bookId) ? BookFactory.LoadSingle(new PostgreSQLConnection(), bookId) : new Book();
            bool done = bool.TryParse(itemResultSet[2][row], out bool d) && d;
            int rating = Int32.TryParse(itemResultSet[3][row], out int r) ? r : 0;
            string notes = itemResultSet[4][row];
            int sortOrder = Int32.TryParse(itemResultSet[5][row], out int s) ? s : 0;
            int listId = Int32.TryParse(itemResultSet[6][row], out int l) ? l : 0;

            return new BookListItem(id, book, done, rating, notes, sortOrder, listId);
        }
    }
}
