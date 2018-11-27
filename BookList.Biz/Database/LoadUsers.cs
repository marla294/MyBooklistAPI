using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class LoadUsers
    {
        public static List<User> LoadAll()
        {
            return LoadByQuery("select * from users order by id;");
        }

        public static User LoadSingle(int id)
        {
            return LoadAll().FirstOrDefault<User>(user => user.Id == id);
        }

        static List<User> LoadByQuery(string sql)
        {
            var userResultSet = ConnectionUtils.ExecuteQuery(new PostgreSQLConnection(), sql);
            var userList = new List<User>();

            for (var i = 0; i < userResultSet[0].Count; i++)
            {
                User user = Int32.TryParse(userResultSet[0][i], out int id)
                    ? new User(id, userResultSet[1][i])
                    : new User();

                userList.Add(user);
            }

            return userList;
        }
    }
}