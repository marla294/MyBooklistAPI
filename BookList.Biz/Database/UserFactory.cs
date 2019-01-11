using System;
using System.Text;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;
using System.Security.Cryptography;

namespace BookList.Biz.Database
{
    public static class UserFactory
    {
        // returns the id of the new user
        // if the username is already taken, returns null
        public static string CreateNewUser(IDbConnection dbConnection, string name, string username, string password)
        {
            if (LoadSingle(username) != null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            if (password.Length < 7)
            {
                return null;
            }

            string userToken = GenerateUserToken();
            var hashedPwd = HashPassword(password);

            dbConnection.Insert("users", new KeyValuePair<string, object>[] {
                                Pairing.Of("name", $"{name}"),
                                Pairing.Of("username", $"{username.ToLower()}"),
                                Pairing.Of("password", $"{hashedPwd}"),
                                Pairing.Of("usertoken", $"{userToken}")
            }).Execute();

            return userToken;
        }

        public static List<User> LoadAll(IDbConnection dbConnection)
        {
            var userResultSet = dbConnection.Take("users").OrderBy("id").Execute();

            var users = new List<User>();

            for (var i = 0; i < userResultSet[0].Count; i++)
            {
                User user = Int32.TryParse(userResultSet[0][i], out int id)
                    ? new User(id, userResultSet[1][i], userResultSet[2][i], userResultSet[3][i], userResultSet[4][i])
                    : new User();

                users.Add(user);
            }

            return users;
        }

        public static User LoadSingle(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var user = LoadAll(new PostgreSQLConnection())
                .FirstOrDefault<User>(u => u.Username == username.ToLower());

            return user ?? null;
        }

        public static User LoadSingleByToken(string userToken)
        {
            if (string.IsNullOrEmpty(userToken))
            {
                return null;
            }

            return LoadAll(new PostgreSQLConnection())
                .FirstOrDefault<User>(u => u.Token == userToken);
        }

        public static bool ConfirmUserPassword(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (password.Length < 7)
            {
                return false;
            }

            var hashedPwd = HashPassword(password);
            var user = LoadSingle(username.ToLower());

            if (user != null)
            {
                return user.Password == hashedPwd ? true : false;
            }
            else
            {
                return false;
            }
        }

        private static string HashPassword(string password)
        {
            string salt = "5%d2$#@asdrewq@334";
            string saltedPassword = password + salt;
            byte[] data = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString());
            }

            return sBuilder.ToString();
        }

        private static string GenerateUserToken()
        {
            var rando = new Random().Next(0, 1000000000).ToString();

            return HashPassword(rando);
        }

        public static void DeleteUser(IDbConnection dbConnection, string userToken)
        {
            if (!string.IsNullOrEmpty(userToken)) {
                dbConnection.Delete("users").Where(Pairing.Of("userToken", userToken)).Execute();
            }

        }
    }
}