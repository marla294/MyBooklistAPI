using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class BookFactory
    {
        public static List<Book> LoadAll(IDbConnection dbConnection)
        {
            var bookResultSet = dbConnection.Select(new string[] { "*" }, "books", "id");

            var books = new List<Book>();

            for (var i = 0; i < bookResultSet[0].Count; i++)
            {
                Book book = Int32.TryParse(bookResultSet[0][i], out int id)
                    ? new Book(id, bookResultSet[1][i], bookResultSet[2][i],
                               bookResultSet[3][i], bookResultSet[4][i])
                    : new Book();

                books.Add(book);
            }

            return books;
        }

        public static Book LoadSingle(int id) 
        {
            return LoadAll(new PostgreSQLConnection()).FirstOrDefault<Book>(book => book.Id == id);
        }
    }
}
