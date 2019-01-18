using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class ListFactory
    {
        // returns the id of the new list
        public static string CreateNewList(IDbConnection dbConnection, string userToken, string listName = "New List")
        {
            string listId;
            int userId;
            User user = UserFactory.LoadSingleByToken(userToken);
            string checkedListName = FactoryUtils.CheckInput(listName, 30);

            // if user doesn't exist don't create
            if (user != null) {
                userId = user.Id;
            } else {
                return null;
            }

            // if listName is whitespace don't create
            if (checkedListName == null)
            {
                return null;
            }

            dbConnection.Insert("lists", new KeyValuePair<string, object>[] {
                                Pairing.Of("name", checkedListName), 
                                Pairing.Of("owner", userId)
            }).Execute();

            listId = dbConnection.Take("lists").OrderBy("id", "desc").Limit(1).Execute()[0][0];

            return listId;
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

        public static void UpdateListName(IDbConnection dbConnection, int id, string listName) 
        {
            string checkedListName = FactoryUtils.CheckInput(listName, 30);

            if (checkedListName != null) {
                dbConnection.Update("lists", Pairing.Of("name", checkedListName)).Where(Pairing.Of("id", id)).Execute();
            }
        }

        public static void DeleteList(IDbConnection dbConnection, int id) 
        {
            dbConnection.Delete("booklist").Where(Pairing.Of("list", id)).Execute();
            dbConnection.Delete("lists").Where(Pairing.Of("id", id)).Execute();
        }
    }
}
