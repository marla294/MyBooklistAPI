using System;
using System.Collections.Generic;
using BookList.Biz.Models;

namespace BookList.Biz.Database
{
    public static class LoadBook
    {
        public static List<BookDTO> LoadAll()
        {
            return LoadByQuery("select * from books order by id;");
        }

        static List<BookDTO> LoadByQuery(string sql)
        {
            var bookResultSet = ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
            var bookList = new List<BookDTO>();

            for (var i = 0; i < bookResultSet[0].Count; i++)
            {
                Book book = Int32.TryParse(bookResultSet[0][i], out int id)
                    ? new Book(id, bookResultSet[1][i], bookResultSet[2][i])
                    : new Book();
                                 
                bookList.Add(book.ToDTO());
            }

            return bookList;
        }
    }
}
