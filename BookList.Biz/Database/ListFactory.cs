using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class ListFactory
    {
        // returns the id of the new list
        public static string CreateNewList()
        {
            return ConnectionUtils.InsertNewList(new PostgreSQLConnection());
        }

        public static List<List> LoadAll()
        {
            var listResultSet = ConnectionUtils.SelectAllLists(new PostgreSQLConnection());
            var listList = new List<List>();

            for (var i = 0; i < listResultSet[0].Count; i++)
            {
                List list = Int32.TryParse(listResultSet[0][i], out int id)
                    ? new List(id, listResultSet[1][i])
                    : new List();

                listList.Add(list);
            }

            return listList;
        }

        public static List LoadSingle(int id)
        {
            return LoadAll().FirstOrDefault<List>(list => list.Id == id);
        }

        public static void UpdateListName(int id, string newName) 
        {
            ConnectionUtils.UpdateListName(new PostgreSQLConnection(), id, newName);
        }

        public static void DeleteList(int id) {
            ConnectionUtils.DeleteList(new PostgreSQLConnection(), id);
        }

        
    }
}
