using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class UserFactory
    {
        public static List<User> LoadAll(IDbConnection dbConnection)
        {
            var userResultSet = dbConnection.Select(new string[] { "*" }, "users", "id");
            var users = new List<User>();

            for (var i = 0; i < userResultSet[0].Count; i++)
            {
                User user = Int32.TryParse(userResultSet[0][i], out int id)
                    ? new User(id, userResultSet[1][i])
                    : new User();

                users.Add(user);
            }

            return users;
        }

        public static User LoadSingle(int id)
        {
            return LoadAll(new PostgreSQLConnection()).FirstOrDefault<User>(user => user.Id == id);
        }

        //static List<User> LoadByQuery(string sql)
        //{
        //    var userResultSet = ConnectionUtils.ExecuteQuery(new PostgreSQLConnection(), sql);
        //    var userList = new List<User>();

        //    for (var i = 0; i < userResultSet[0].Count; i++)
        //    {
        //        User user = Int32.TryParse(userResultSet[0][i], out int id)
        //            ? new User(id, userResultSet[1][i])
        //            : new User();

        //        userList.Add(user);
        //    }

        //    return userList;
        //}
    }
}