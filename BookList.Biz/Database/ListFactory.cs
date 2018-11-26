using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class ListFactory
    {
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

        public static string CreateNewList()
        {
            //var sql = "insert into lists (name) values ('New List')";
            ConnectionUtils.InsertNewList(new PostgreSQLConnection());

            var sql = "select id from lists order by id desc limit 1";
            var id = ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql)[0][0];

            sql = "insert into booklist(book, username, done, rating, notes, " +
                $"sortorder, list) values (null, 1, false, null, '', 0, {id})";
            ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);

            return id;
        }

        public static void DeleteList(int id) {
            ConnectionUtils.DeleteList(new PostgreSQLConnection(), id);
        }

        
    }
}
