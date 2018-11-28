using System;
using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public static class ConnectionUtils
    {
        public static List<List<string>> ExecuteQuery(IDbConnection dbConnection, string sql)
        {
            return dbConnection.ExecuteQuery(sql);
        }

        public static string InsertNewList(IDbConnection dbConnection)
        {
            dbConnection.Insert("lists", 
                                new ColumnValuePairing("name", "New List")
                               );

            var id = dbConnection.Select(new string[] { "*" }, "lists", "id", "desc", 1)[0][0];

            Int32.TryParse(id, out int idConverted);

            dbConnection.Insert("booklist", 
                                new ColumnValuePairing("book", null), 
                                new ColumnValuePairing("username", 1), 
                                new ColumnValuePairing("done", false),
                                new ColumnValuePairing("rating", null), 
                                new ColumnValuePairing("notes", ""),
                                new ColumnValuePairing("sortorder", 0), 
                                new ColumnValuePairing("list", idConverted)
                               );

            return id;

        }

        public static List<List<string>> SelectAllLists(IDbConnection dbConnection)
        {
            return dbConnection.Select(new string[] { "*" }, "lists", "id");
        }

        public static void UpdateListName(IDbConnection dbConnection, int id, string newName)
        {
            dbConnection.Update("lists", "name", newName, "and", new ColumnValuePairing("id", id));
        }

        public static void DeleteList(IDbConnection dbConnection, int id)
        {
            dbConnection.Delete("booklist", new ColumnValuePairing("list", id));
            dbConnection.Delete("lists", new ColumnValuePairing("id", id));
        }

        public static List<List<string>> CreateEmptyResultSet(int numCols)
        {
            List<List<string>> resultSet = new List<List<string>>();

            for (var col = 0; col < numCols; col++)
            {
                resultSet.Add(new List<string>());
            }

            return resultSet;
        }


    }
}