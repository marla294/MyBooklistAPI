using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class ListFactory
    {
        // returns the id of the new list
        public static string CreateNewList(IDbConnection dbConnection)
        {
            string id;

            dbConnection.Insert("lists", new ColumnValuePairing[] {
                                new ColumnValuePairing("name", "New List")
            });

            id = dbConnection.Select(new string[] { "*" }, "lists", "id", "desc", 1)[0][0];

            Int32.TryParse(id, out int idConverted);

            dbConnection.Insert("booklist", new ColumnValuePairing[] {
                                new ColumnValuePairing("book", null),
                                new ColumnValuePairing("username", 1),
                                new ColumnValuePairing("done", false),
                                new ColumnValuePairing("rating", null),
                                new ColumnValuePairing("notes", ""),
                                new ColumnValuePairing("sortorder", 0),
                                new ColumnValuePairing("list", idConverted)
            });

            return id;
        }

        public static List<List> LoadAll(IDbConnection dbConnection)
        {
            var listResultSet = dbConnection.Select(new string[] { "*" }, "lists", "id");

            var lists = new List<List>();

            for (var i = 0; i < listResultSet[0].Count; i++)
            {
                List list = Int32.TryParse(listResultSet[0][i], out int id)
                    ? new List(id, listResultSet[1][i])
                    : new List();

                lists.Add(list);
            }

            return lists;
        }

        public static List LoadSingle(IDbConnection dbConnection, int id)
        {
            return LoadAll(dbConnection).FirstOrDefault<List>(list => list.Id == id);
        }

        public static void UpdateListName(IDbConnection dbConnection, int id, string newName) 
        {
            dbConnection.Update("lists", 
                                new ColumnValuePairing("name", newName),
                                "and", 
                                new ColumnValuePairing("id", id)
                               );
        }

        public static void DeleteList(IDbConnection dbConnection, int id) 
        {
            dbConnection.Delete("booklist", "and", new ColumnValuePairing("list", id));
            dbConnection.Delete("lists", "and", new ColumnValuePairing("id", id));
        }

        
    }
}
