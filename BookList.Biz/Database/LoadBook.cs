using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class LoadBook
    {
        public static List<Book> LoadAll()
        {
            return LoadByQuery("select * from books order by id;");
        }

        public static Book LoadSingle(int id) 
        {
            return LoadAll().FirstOrDefault<Book>(book => book.Id == id);
        }

        static List<Book> LoadByQuery(string sql)
        {
            var bookResultSet = ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
            var bookList = new List<Book>();

            for (var i = 0; i < bookResultSet[0].Count; i++)
            {
                Book book = Int32.TryParse(bookResultSet[0][i], out int id)
                    ? new Book(id, bookResultSet[1][i], bookResultSet[2][i], 
                               bookResultSet[3][i], bookResultSet[4][i])
                    : new Book();
                                 
                bookList.Add(book);
            }

            return bookList;
        }
    }
}
