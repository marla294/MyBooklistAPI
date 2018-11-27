using System;
using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public static class ConnectionUtils
    {
        public static List<List<string>> ExecuteCommand(IDbConnection dbConnection, string command)
        {
            return dbConnection.ExecuteCommand(command);
        }

        public static string InsertNewList(IDbConnection dbConnection)
        {
            dbConnection.Insert("lists", 
                                new InsertValues("name", "New List")
                               );

            var id = dbConnection.Select(new string[] { "*" }, "lists", "id", "desc", 1)[0][0];

            Int32.TryParse(id, out int idConverted);

            dbConnection.Insert("booklist", 
                                new InsertValues("book", null), 
                                new InsertValues("username", 1), 
                                new InsertValues("done", false),
                                new InsertValues("rating", null), 
                                new InsertValues("notes", ""),
                                new InsertValues("sortorder", 0), 
                                new InsertValues("list", idConverted)
                               );

            return id;

        }

        public static List<List<string>> SelectAllLists(IDbConnection dbConnection)
        {
            return dbConnection.Select(new string[] { "*" }, "lists", "id");
        }

        public static void UpdateListName(IDbConnection dbConnection, int id, string newName)
        {
            dbConnection.Update("lists", "name", newName, "and", new WhereValues("id", id.ToString()));
        }

        public static void DeleteList(IDbConnection dbConnection, int id)
        {
            dbConnection.Delete("booklist", new WhereValues("list", id.ToString()));
            dbConnection.Delete("lists", new WhereValues("id", id.ToString()));
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