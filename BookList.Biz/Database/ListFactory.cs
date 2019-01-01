using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class ListFactory
    {
        // returns the id of the new list
        public static string CreateNewList(IDbConnection dbConnection, string listName = "New List")
        {
            string id;

            dbConnection.Insert("lists", new KeyValuePair<string, object>[] {
                                Pairing.Of("name", $"{listName}"), 
                                Pairing.Of("owner", 1)
            }).Execute();

            id = dbConnection.Take("lists").OrderBy("id", "desc").Limit(1).Execute()[0][0];

            return id;
        }

        public static List<List> LoadAll(IDbConnection dbConnection)
        {
            var listResultSet = dbConnection.Take("lists").OrderBy("id", "asc").Execute();

            var lists = new List<List>();

            for (var i = 0; i < listResultSet[0].Count; i++)
            {
                User user = new User();

                if (Int32.TryParse(listResultSet[2][i], out int userId)) {
                    user = UserFactory.LoadAll(dbConnection).FirstOrDefault(u => u.Id == userId);
                }

                List list = Int32.TryParse(listResultSet[0][i], out int id)
                    ? new List(id, 
                               listResultSet[1][i], 
                               user
                              )
                    : new List();

                lists.Add(list);
            }

            return lists;
        }

        public static List LoadSingle(IDbConnection dbConnection, int id)
        {
            return LoadAll(dbConnection).FirstOrDefault<List>(list => list.Id == id);
        }

        public static List<List> LoadByUserId(IDbConnection dbConnection, int userId)
        {
            List<List> AllLists = LoadAll(dbConnection);
            List<List> FilteredLists = new List<List>();

            for (var i = 0; i < AllLists.Count; i++) {
                if (AllLists[i].Owner.Id == userId) {
                    FilteredLists.Add(AllLists[i]);
                }
            }

            return FilteredLists;
        }

        public static void UpdateListName(IDbConnection dbConnection, int id, string newName) 
        {
            dbConnection.Update("lists", Pairing.Of("name", newName)).Where(Pairing.Of("id", id)).Execute();
        }

        public static void DeleteList(IDbConnection dbConnection, int id) 
        {
            dbConnection.Delete("booklist").Where(Pairing.Of("list", id)).Execute();
            dbConnection.Delete("lists").Where(Pairing.Of("id", id)).Execute();
        }

        
    }
}
