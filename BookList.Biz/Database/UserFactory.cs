using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class UserFactory
    {
        // returns the id of the new user
        public static string CreateNewUser(IDbConnection dbConnection, string name, string username, string password)
        {
            string id;

            dbConnection.Insert("users", new KeyValuePair<string, object>[] {
                                Pairing.Of("name", $"{name}"),
                                Pairing.Of("username", $"{username}"),
                                Pairing.Of("password", $"{password}")
            }).Execute();

            id = dbConnection.Take("users").OrderBy("id", "desc").Limit(1).Execute()[0][0];

            return id;
        }

        public static List<User> LoadAll(IDbConnection dbConnection)
        {
            var userResultSet = dbConnection.Take("users").OrderBy("id").Execute();

            var users = new List<User>();

            for (var i = 0; i < userResultSet[0].Count; i++)
            {
                User user = Int32.TryParse(userResultSet[0][i], out int id)
                    ? new User(id, userResultSet[1][i], userResultSet[2][i], userResultSet[3][i])
                    : new User();

                users.Add(user);
            }

            return users;
        }

        public static User LoadSingle(string username, string password)
        {
            return LoadAll(new PostgreSQLConnection())
                .FirstOrDefault<User>(user => user.Username == username && user.Password == password);
        }

        public static void DeleteUser(IDbConnection dbConnection, int id)
        {
            dbConnection.Delete("users").Where(Pairing.Of("id", id)).Execute();
        }
    }
}