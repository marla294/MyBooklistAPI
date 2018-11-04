using System;
using System.Collections.Generic;
using BookList.Biz.Models;

namespace BookList.Biz.Database
{
    public static class LoadUsers
    {
        public static List<UserDTO> LoadAll()
        {
            return LoadByQuery("select * from users order by id;");
        }

        static List<UserDTO> LoadByQuery(string sql)
        {
            var userResultSet = ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
            var userList = new List<UserDTO>();

            for (var i = 0; i < userResultSet[0].Count; i++)
            {
                User user = Int32.TryParse(userResultSet[0][i], out int id)
                    ? new User(id, userResultSet[1][i])
                    : new User();

                userList.Add(user.ToDTO());
            }

            return userList;
        }
    }
}